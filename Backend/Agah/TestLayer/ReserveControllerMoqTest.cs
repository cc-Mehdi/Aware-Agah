using Agah.Controllers;
using Agah.Services;
using Agah.Services.Interfaces;
using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace TestLayer
{
    public class ReserveControllerMoqTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IReserveRepository> _mockReserveRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IProductLogRepository> _mockProductLogRepository;
        private readonly Mock<IAlarmRepository> _mockAlarmRepository;
        private readonly ReserveController _controller;


        public ReserveControllerMoqTest(IEmailService emailService, ReserveService reserveService)
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockReserveRepository = new Mock<IReserveRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockProductLogRepository = new Mock<IProductLogRepository>();
            _mockAlarmRepository = new Mock<IAlarmRepository>();

            _mockUnitOfWork.Setup(u => u.ReserveRepository).Returns(_mockReserveRepository.Object);
            _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepository.Object);
            _mockUnitOfWork.Setup(u => u.ProductLogRepository).Returns(_mockProductLogRepository.Object);
            _mockUnitOfWork.Setup(u => u.AlarmRepository).Returns(_mockAlarmRepository.Object);

            _controller = new ReserveController(_mockUnitOfWork.Object, emailService, reserveService);
        }

        // =========> GetReserves Endpoint
        [Fact]
        public async Task GetReserves_ReturnsReserves_WhenExist()
        {
            // Arrange
            var fakeUsers = FakeDataGenerator.GenerateUsers(2);
            var fakeProducts = FakeDataGenerator.GenerateProducts(2);
            var fakeAlarms = FakeDataGenerator.GenerateAlarms(2);
            var fakeReserves = FakeDataGenerator.GenerateReserves(2, fakeUsers, fakeProducts, fakeAlarms);

            _mockReserveRepository.Setup(repo => repo.GetAllByFilterAsync(It.IsAny<Expression<Func<Reserve, bool>>>()))
                .ReturnsAsync(fakeReserves);

            // Act
            var result = await _controller.GetReserves();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var reservesList = Assert.IsType<List<Reserve>>(okResult.Value);

            Assert.Equal(fakeReserves.Count, reservesList.Count);
            Assert.All(reservesList, reserve => Assert.False(reserve.IsSent));
        }

        [Fact]
        public async Task GetReserves_NoReserves_ReturnsEmptyList()
        {
            // Arrange
            var fakeReserves = new List<Reserve>();

            _mockReserveRepository.Setup(repo => repo.GetAllByFilterAsync(It.IsAny<Expression<Func<Reserve, bool>>>()))
                .ReturnsAsync(fakeReserves);

            // Act
            var result = await _controller.GetReserves();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var reservesList = Assert.IsType<List<Reserve>>(okResult.Value);

            Assert.Equal(fakeReserves.Count, reservesList.Count);
        }
        // =========> End GetReserves Endpoint

        // =========> CheckPriceInReserveds Endpoint
        [Fact]
        public async Task CheckPriceInReserveds_ReturnsBadRequest_WhenReserveNotFound()
        {
            // Arrange
            int userId = 1;
            _mockReserveRepository
                .Setup(repo => repo.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Reserve, bool>>>(), // فیلتر
                    It.IsAny<Expression<Func<Reserve, object>>>() // شامل کردن (include)
                ))
                .ReturnsAsync((Reserve)null);

            // Act
            var result = await _controller.CheckPriceInReserveds(userId);

            // Assert
            TestHelper.AssertBadRequestWithMessage(result, "رزرو بازه زمانی برای کاربر انتخابی یافت نشد");
        }

        [Fact]
        public async Task CheckPriceInReserveds_ReturnsOk_WhenPriceIsWithinRange()
        {
            // Arrange
            var fakeUsers = FakeDataGenerator.GenerateUsers(1);
            var fakeProducts = FakeDataGenerator.GenerateProducts(1);
            var fakeAlarms = new List<Alarm>
            {
                new Alarm { Id = 1, EnglishName = "Alert", IsActive = true }
            };
            var fakeProductLogs = FakeDataGenerator.GenerateProductLogs(1, fakeProducts);
            fakeProductLogs[0].Price = 150;
            var fakeReserve = new Reserve
            {
                User_Id = fakeUsers[0].Id,
                User = fakeUsers[0],
                Product_Id = fakeProducts[0].Id,
                Product = fakeProducts[0],
                Alarm_Id = fakeAlarms[0].Id,
                Alarm = fakeAlarms[0],
                MinPrice = 100,
                MaxPrice = 200,
                IsSent = false
            };



            _mockReserveRepository
                .Setup(repo => repo.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Reserve, bool>>>(),
                    It.Is<Expression<Func<Reserve, object>>[]>(includes =>
                        includes.Length == 3 &&
                        includes.Any(e => e.Body.ToString().Contains("User")) &&
                        includes.Any(e => e.Body.ToString().Contains("Product")) &&
                        includes.Any(e => e.Body.ToString().Contains("Alarm"))
                    )
                ))
                .ReturnsAsync(fakeReserve);

            _mockProductRepository.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(fakeProducts[0]);

            _mockAlarmRepository.Setup(repo => repo.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Alarm, bool>>>(exp => exp.Compile()(fakeAlarms[0])),
                It.IsAny<Expression<Func<Alarm, object>>>()
            )).ReturnsAsync(fakeAlarms[0]);

            _mockProductLogRepository
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<Expression<Func<ProductLog, object>>>() // شامل کردن (include)
                ))
                .ReturnsAsync(fakeProductLogs);



            // Act
            var result = await _controller.CheckPriceInReserveds(fakeUsers[0].Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            TestHelper.AssertOkWithMessage(result, "قیمت در بازه رزرو شده میباشد");
        }

        // TODO : this test work on one endpoint and that endpoint call another service and for implement this test needs to select best way
        //[Fact]
        //public async Task CheckPriceInReserveds_ReturnsOk_WhenPriceIsOutsideRangeAndNotificationSent()
        //{
        //    // Arrange
        //    var fakeUsers = FakeDataGenerator.GenerateUsers(1);
        //    var fakeProducts = FakeDataGenerator.GenerateProducts(1);
        //    var fakeAlarms = FakeDataGenerator.GenerateAlarms(1);
        //    var fakeProductLogs = FakeDataGenerator.GenerateProductLogs(1, fakeProducts);
        //    var fakeReserve = new Reserve
        //    {
        //        User_Id = fakeUsers[0].Id,
        //        User = fakeUsers[0],
        //        Product_Id = fakeProducts[0].Id,
        //        Product = fakeProducts[0],
        //        Alarm_Id = fakeAlarms[0].Id,
        //        Alarm = fakeAlarms[0],
        //        MinPrice = 100,
        //        MaxPrice = 200,
        //        IsSent = false
        //    };

        //    // تنظیم قیمت خارج از بازه
        //    fakeProductLogs[0].Price = 250; // قیمت خارج از بازه

        //    _mockReserveRepository
        //        .Setup(repo => repo.GetFirstOrDefaultAsync(
        //            It.IsAny<Expression<Func<Reserve, bool>>>(),
        //            It.Is<Expression<Func<Reserve, object>>[]>(includes =>
        //                includes.Length == 3 &&
        //                includes.Any(e => e.Body.ToString().Contains("User")) &&
        //                includes.Any(e => e.Body.ToString().Contains("Product")) &&
        //                includes.Any(e => e.Body.ToString().Contains("Alarm"))
        //            )
        //        ))
        //        .ReturnsAsync(fakeReserve);

        //    _mockProductRepository
        //        .Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
        //        .ReturnsAsync(fakeProducts[0]);

        //    _mockProductLogRepository
        //        .Setup(repo => repo.GetAllAsync(
        //            It.IsAny<Expression<Func<ProductLog, object>>>() // شامل کردن (include)
        //        ))
        //        .ReturnsAsync(fakeProductLogs);

        //    // شبیه‌سازی ارسال موفقیت‌آمیز اطلاع‌رسانی
        //    _mockReserveRepository
        //        .Setup(repo => repo.UpdateAsync(It.IsAny<Reserve>()))
        //        .Returns(Task.CompletedTask);

        //    _mockUnitOfWork
        //        .Setup(u => u.SaveAsync())
        //        .Returns(Task.CompletedTask);

        //    // Act
        //    var result = await _controller.CheckPriceInReserveds(fakeUsers[0].Id);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    TestHelper.AssertOkWithMessage(result, "پیام اطلاع رسانی در صف ارسال قرار گرفت");

        //    // بررسی اینکه وضعیت IsSent به درستی به true تغییر کرده است
        //    Assert.True(fakeReserve.IsSent);
        //} 

        [Fact]
        public async Task CheckPriceInReserveds_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var fakeUsers = FakeDataGenerator.GenerateUsers(1);

            // شبیه‌سازی خطا در هنگام فراخوانی متد
            _mockReserveRepository
                .Setup(repo => repo.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Reserve, bool>>>(),
                    It.Is<Expression<Func<Reserve, object>>[]>(includes =>
                        includes.Length == 3 &&
                        includes.Any(e => e.Body.ToString().Contains("User")) &&
                        includes.Any(e => e.Body.ToString().Contains("Product")) &&
                        includes.Any(e => e.Body.ToString().Contains("Alarm"))
                    )
                ))
                .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.CheckPriceInReserveds(fakeUsers[0].Id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            TestHelper.AssertBadRequestWithMessage(result, "عملیات با خطا مواجه شد\nخطا : Test Exception");
        }
        // =========> End CheckPriceInReserveds Endpoint


    }
}
