using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestLayer
{
    public static class TestHelper
    {
        public static void SetupNotificationRepo(Mock<INotification_UserRepository> mockRepo, List<Notification_User> fakeNotifications)
        {
            mockRepo.Setup(repo => repo.GetAllByFilterAsync(
                It.IsAny<Expression<Func<Notification_User, bool>>>(),
                It.IsAny<Expression<Func<Notification_User, object>>[]>()
            )).ReturnsAsync(fakeNotifications);
        }

        public static void SetupUserRepo(Mock<IUnitOfWork> mockUnitOfWork, User user)
        {
            mockUnitOfWork.Setup(u => u.UserRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                          .ReturnsAsync(user);
        }

        public static void SetupNotificationUpdate(Mock<IUnitOfWork> mockUnitOfWork)
        {
            mockUnitOfWork.Setup(u => u.Notification_UserRepository.UpdateAsync(It.IsAny<Notification_User>()))
                          .Returns(Task.CompletedTask);
            mockUnitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
        }

        public static void AssertBadRequestWithMessage(IActionResult result, string expectedMessage)
        {
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = JsonConvert.DeserializeObject<ResponseStatus>(JsonConvert.SerializeObject(badRequestResult.Value));
            Assert.NotNull(errorResponse);
            Assert.Equal(expectedMessage, errorResponse.StatusMessage);
        }
        public static List<T> ExtractListFromResult<T>(OkObjectResult okResult)
        {
            Assert.NotNull(okResult.Value);

            if (okResult.Value is IEnumerable<T> enumerable)
            {
                return enumerable.ToList();
            }

            throw new InvalidCastException($"Expected value of type IEnumerable<{typeof(T).Name}>, but got {okResult.Value.GetType().Name}.");
        }
    }
}
