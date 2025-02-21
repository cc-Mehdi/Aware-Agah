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
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly NotificationController _controller;

        public NotificationControllerMoqTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockNotificationUserRepository = new Mock<INotification_UserRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUnitOfWork.Setup(u => u.Notification_UserRepository).Returns(_mockNotificationUserRepository.Object);
            _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepository.Object);
            _controller = new NotificationController(_mockUnitOfWork.Object);
        }

        // =========> GetNotifications Endpoint
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
        // =========> End GetNotifications Endpoint

        // =========> ReadAllNotifications Endpoint
        [Fact]
        public async Task ReadAllNotifications_UserNotFound_ReturnsBadRequest()
        {
            TestHelper.SetupRepository(_mockUnitOfWork, (User)null);
            var result = await _controller.ReadAllNotifications(1);
            TestHelper.AssertBadRequestWithMessage(result, "کاربر مورد نظر یافت نشد");
        }

        [Fact]
        public async Task ReadAllNotifications_NoNotificationsFound_ReturnsBadRequest()
        {
            var fakeUsers = FakeDataGenerator.GenerateUsers(1);
            var fakeNotifications = new List<Notification_User>();

            _mockUserRepository.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(fakeUsers[0]);
            _mockNotificationUserRepository.Setup(repo => repo.GetAllByFilterAsync(It.IsAny<Expression<Func<Notification_User, bool>>>())).ReturnsAsync(fakeNotifications);

            var result = await _controller.ReadAllNotifications(fakeUsers[0].Id);


            TestHelper.AssertBadRequestWithMessage(result, "اعلانی برای کاربر مورد نظر یافت نشد");
        }

        [Fact]
        public async Task ReadAllNotifications_MarksAllAsRead_ReturnsOk()
        {
            var fakeUsers = FakeDataGenerator.GenerateUsers(1);
            var fakeNotifications = FakeDataGenerator.GenerateNotificationUsers(1, fakeUsers);

            _mockUserRepository.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(fakeUsers[0]);
            _mockNotificationUserRepository.Setup(repo => repo.GetAllByFilterAsync(It.IsAny<Expression<Func<Notification_User, bool>>>())).ReturnsAsync(fakeNotifications);

            var result = await _controller.ReadAllNotifications(fakeUsers[0].Id);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedNotifications = TestHelper.ExtractListFromResult<Notification_User>(okResult);

            Assert.All(returnedNotifications, n => Assert.True(n.IsRead));
        }

        [Fact]
        public async Task ReadAllNotifications_ThrowsException_ReturnsBadRequest()
        {
            var fakeUsers = FakeDataGenerator.GenerateUsers(1);

            _mockUserRepository.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(fakeUsers[0]);
            _mockNotificationUserRepository.Setup(repo => repo.GetAllByFilterAsync(It.IsAny<Expression<Func<Notification_User, bool>>>())).ThrowsAsync(new Exception("Test Exception"));

            var result = await _controller.ReadAllNotifications(fakeUsers[0].Id);
            TestHelper.AssertBadRequestWithMessage(result, "عملیات با خطا مواجه شد\nخطا : Test Exception");
        }
        // =========> End ReadAllNotifications Endpoint

    }
}
