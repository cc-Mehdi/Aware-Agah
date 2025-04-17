using Agah.Services.Interfaces;
using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Agah.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ResponseStatus> UpdateProductLog() // this method responsible for checking product price. if product changed, add new log and else no changes happend.
        {
            string url = "https://milli.gold/api/v1/public/milli-price/detail";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

                    if (apiResponse != null && apiResponse.FirstOrDefault().Value != null)
                    {
                        string responseKey = apiResponse.FirstOrDefault().Key;
                        string responseValue = apiResponse.FirstOrDefault().Value;

                        // Get last submited product log
                        var ProductLogsList = await _unitOfWork.ProductLogRepository.GetAllByFilterAsync(u => u.Product.EnglishName == responseKey);
                        var lastProductLog = ProductLogsList.OrderByDescending(u => u.CreatedAt).FirstOrDefault();

                        decimal.TryParse(responseValue, out decimal price); // Get current product price

                        if (lastProductLog == null) // if not save any product log for this product
                        {
                            var product = await _unitOfWork.ProductRepository.GetFirstOrDefaultAsync(u => u.EnglishName == responseKey);
                            int productId = product.Id; // Get product id

                            if (productId == 0)
                                return new ResponseStatus() { StatusCode = 400, StatusMessage = "برای محصول انتخاب شده گزارش قیمتی یافت نشد" };


                            await _unitOfWork.ProductLogRepository.AddAsync(new ProductLog()
                            {
                                Product_Id = productId,
                                Price = price,
                                CreatedAt = DateTime.Now
                            });

                            await _unitOfWork.SaveAsync();

                            return new ResponseStatus() { StatusCode = 200, StatusMessage = $"گزارش قیمت جدیدی برای محصول {responseKey} ثبت شد.  {responseKey} => {responseValue}" };
                        }
                        else // if product log for this product was submited
                        {
                            if (lastProductLog.Price != price) // check if price changed after last time
                            {
                                await _unitOfWork.ProductLogRepository.AddAsync(new ProductLog()
                                {
                                    Product_Id = lastProductLog.Product_Id,
                                    Price = price,
                                    CreatedAt = DateTime.Now
                                });

                                await _unitOfWork.SaveAsync();

                                return new ResponseStatus() { StatusCode = 200, StatusMessage = $"گزارش قیمت برای محصول {responseKey} بروزرسانی شد. {lastProductLog.Price} => {responseValue}" };
                            }

                            return new ResponseStatus() { StatusCode = 400, StatusMessage = $"تغییر قیمتی برای محصول {responseKey} وجود ندارد.  {responseKey} => {responseValue}" };
                        }
                    }
                    return new ResponseStatus() { StatusCode = 400, StatusMessage = "دریافت گزارش قیمت با شکست مواجه شد" };
                }
                catch (Exception ex)
                {
                    return new ResponseStatus() { StatusCode = 400, StatusMessage = $"ما با خطای {ex.Message} رو به رو شده ایم 😖" };
                }
            }
        }

    }
}
