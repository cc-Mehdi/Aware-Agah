using Agah.Controllers;
using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestLayer
{
    public class NotificationControllerMoqTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<INotification_UserRepository> _mockNotificationUserRepository;
        private readonly NotificationController _controller;

        public NotificationControllerMoqTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockNotificationUserRepository = new Mock<INotification_UserRepository>();

            _mockUnitOfWork.Setup(u => u.Notification_UserRepository).Returns(_mockNotificationUserRepository.Object);

            _controller = new NotificationController(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetNotifications_ShouldReturnNotifications_WhenNotificationsExist()
        {
            // Arrange
            var fakeUsers = FakeDataGenerator.GenerateUsers(2);
            var fakeNotifications = FakeDataGenerator.GenerateNotificationUsers(2, fakeUsers);

            // شبیه‌سازی متد GetAllByFilterAsync
            _mockNotificationUserRepository
                .Setup(repo => repo.GetAllByFilterAsync(
                    It.IsAny<Expression<Func<Notification_User, bool>>>(),
                    It.IsAny<Expression<Func<Notification_User, object>>[]>()
                ))
                .ReturnsAsync(fakeNotifications);

            // Act
            var result = await _controller.GetNotifications(fakeUsers[0].Id, "Alert");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // ✅ تبدیل OrderedEnumerable به لیست
            var orderedNotifications = okResult.Value as IOrderedEnumerable<Notification_User>;
            Assert.NotNull(orderedNotifications);

            var notificationsList = orderedNotifications.ToList(); // تبدیل به لیست
            Assert.Equal(2, notificationsList.Count);

            // بررسی مرتب‌سازی (اگر نیاز است)
            Assert.True(notificationsList[0].CreatedAt >= notificationsList[1].CreatedAt); // به عنوان مثال: مرتب‌سازی نزولی
        }

        [Fact]
        public async Task GetNotifications_WithInvalidAlarmType_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            var alarmType = "InvalidType";

            // Act
            var result = await _controller.GetNotifications(userId, alarmType);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // ✅ تبدیل Value به کلاس مدل
            var errorResponse = JsonConvert.DeserializeObject<ResponseStatus>(JsonConvert.SerializeObject(badRequestResult.Value));
            Assert.NotNull(errorResponse);
            Assert.Equal("امکان ارسال اعلان با روش انتخاب شده وجود ندارد", errorResponse.StatusMessage);
        }

        [Fact]
        public async Task GetNotifications_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            var alarmType = "Alert";

            // شبیه‌سازی خطا در متد GetAllByFilterAsync
            _mockNotificationUserRepository
                .Setup(repo => repo.GetAllByFilterAsync(
                    It.IsAny<Expression<Func<Notification_User, bool>>>(),
                    It.IsAny<Expression<Func<Notification_User, object>>[]>()
                ))
                .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.GetNotifications(userId, alarmType);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // ✅ تبدیل Value به کلاس مدل
            var errorResponse = JsonConvert.DeserializeObject<ResponseStatus>(JsonConvert.SerializeObject(badRequestResult.Value));
            Assert.NotNull(errorResponse);
            Assert.Equal("عملیات با خطا مواجه شد\nخطا : Test Exception", errorResponse.StatusMessage);
        }

        [Fact]
        public async Task GetNotifications_WithNoNotifications_ReturnsEmptyList()
        {
            // Arrange
            var userId = 1;
            var alarmType = "Alert";
            var emptyNotifications = new List<Notification_User>();

            // شبیه‌سازی متد GetAllByFilterAsync
            _mockNotificationUserRepository
                .Setup(repo => repo.GetAllByFilterAsync(
                    It.IsAny<Expression<Func<Notification_User, bool>>>(),
                    It.IsAny<Expression<Func<Notification_User, object>>[]>()
                ))
                .ReturnsAsync(emptyNotifications);

            // Act
            var result = await _controller.GetNotifications(userId, alarmType);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // ✅ تبدیل OrderedEnumerable به لیست
            var orderedNotifications = okResult.Value as IOrderedEnumerable<Notification_User>;
            Assert.NotNull(orderedNotifications);

            var notificationsList = orderedNotifications.ToList(); // تبدیل به لیست
            Assert.Equal(0, notificationsList.Count);
        }
    }
}
