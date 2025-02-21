using Agah.Controllers;
using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Linq.Expressions;

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

        [Theory]
        [InlineData(1, "Alert")]
        public async Task GetNotifications_ReturnsNotifications_WhenExist(int userId, string alarmType)
        {
            var fakeUsers = FakeDataGenerator.GenerateUsers(2);
            var fakeNotifications = FakeDataGenerator.GenerateNotificationUsers(2, fakeUsers);

            TestHelper.SetupNotificationRepo(_mockNotificationUserRepository, fakeNotifications);

            var result = await _controller.GetNotifications(userId, alarmType);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var notificationsList = TestHelper.ExtractListFromResult<Notification_User>(okResult);

            Assert.Equal(2, notificationsList.Count);
            Assert.True(notificationsList[0].CreatedAt >= notificationsList[1].CreatedAt);
        }

        [Theory]
        [InlineData(1, "InvalidType")]
        public async Task GetNotifications_InvalidAlarmType_ReturnsBadRequest(int userId, string alarmType)
        {
            var result = await _controller.GetNotifications(userId, alarmType);
            TestHelper.AssertBadRequestWithMessage(result, "امکان ارسال اعلان با روش انتخاب شده وجود ندارد");
        }

        [Fact]
        public async Task GetNotifications_ThrowsException_ReturnsBadRequest()
        {
            _mockNotificationUserRepository.Setup(repo => repo.GetAllByFilterAsync(It.IsAny<Expression<Func<Notification_User, bool>>>(), It.IsAny<Expression<Func<Notification_User, object>>[]>())).ThrowsAsync(new Exception("Test Exception"));
            var result = await _controller.GetNotifications(1, "Alert");
            TestHelper.AssertBadRequestWithMessage(result, "عملیات با خطا مواجه شد\nخطا : Test Exception");
        }

        [Fact]
        public async Task GetNotifications_NoNotifications_ReturnsEmptyList()
        {
            TestHelper.SetupNotificationRepo(_mockNotificationUserRepository, new List<Notification_User>());
            var result = await _controller.GetNotifications(1, "Alert");
            var okResult = Assert.IsType<OkObjectResult>(result);
            var notificationsList = TestHelper.ExtractListFromResult<Notification_User>(okResult);
            Assert.Empty(notificationsList);
        }

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
