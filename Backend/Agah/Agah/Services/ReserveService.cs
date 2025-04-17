using Agah.Controllers;
using Agah.Services.Interfaces;
using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Agah.Services
{
    public class ReserveService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;


        public ReserveService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }


        public async Task<IEnumerable<Reserve>> GetReserves()
        {
            var reservesUserIdList = await _unitOfWork.ReserveRepository.GetAllByFilterAsync(u => u.IsSent != true);
            if (reservesUserIdList != null)
                return reservesUserIdList;
            else
                return null;
        }

        public async Task<ResponseStatus> CheckPriceInReserveds(int userId)
        {
            try
            {
                var reserve = await _unitOfWork.ReserveRepository.GetFirstOrDefaultAsync(u => u.User_Id == userId, u => u.User, u => u.Product, u => u.Alarm);
                if (reserve == null)
                    return new ResponseStatus { StatusCode = 400, StatusMessage = "رزرو بازه زمانی برای کاربر انتخابی یافت نشد" };

                var product = await _unitOfWork.ProductRepository.GetFirstOrDefaultAsync(u => u.Id == reserve.Product_Id);
                string productName = product?.PersianName ?? "";

                // get last products price
                var lastProductsLog = await _unitOfWork.ProductLogRepository.GetAllAsync(includeProperties: u => u.Product);
                decimal lastProductsPrice = lastProductsLog.Where(u => u.Product_Id == reserve.Product_Id)
                    .OrderByDescending(u => u.CreatedAt)
                    .GroupBy(u => u.Product_Id)
                    .Select(g => g.First())
                    .Select(u => u.Price)
                    .FirstOrDefault();


                if (lastProductsPrice < reserve.MinPrice || lastProductsPrice > reserve.MaxPrice)
                {
                    NotificationService notificationService = new NotificationService(_unitOfWork, _emailService);
                    ResponseStatus response = await notificationService.SendNotification(new Notification_MessageOptions()
                    {
                        User = reserve.User,
                        NotificationType = reserve.Alarm.EnglishName,
                        MessageSubject = $"قیمت {reserve.Product.PersianName} بیش از حد تغییر کرده است!",
                        MessageContent = $"محصول : {productName}\nقیمت فعلی : {lastProductsPrice.ToString("N0")}\nبازه رزرو شده : {reserve.MinPrice.ToString("N0")} - {reserve.MaxPrice.ToString("N0")}",
                    });
                    if (response.StatusCode == 200)
                    {
                        // change status of isSent in reserve
                        reserve.IsSent = true;
                        await _unitOfWork.ReserveRepository.UpdateAsync(reserve);
                        await _unitOfWork.SaveAsync();
                        return new ResponseStatus { StatusCode = 200, StatusMessage = "پیام اطلاع رسانی در صف ارسال قرار گرفت" };
                    }
                    else
                        return new ResponseStatus { StatusCode = 400, StatusMessage = $"عملیات با خطا مواجه شد" };
                }
                return new ResponseStatus { StatusCode = 200, StatusMessage = "قیمت در بازه رزرو شده میباشد" };
            }
            catch (Exception ex)
            {
                return new ResponseStatus { StatusCode = 400, StatusMessage = $"عملیات با خطا مواجه شد\nخطا : {ex.Message}" };
            }
        }

    }
}
