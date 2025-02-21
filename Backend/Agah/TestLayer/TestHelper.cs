using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
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

        public static void SetupRepository<T>(Mock<IUnitOfWork> mockUnitOfWork, T entity) where T : class
        {
            var repoMock = new Mock<IRepository<T>>();
            repoMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<T, bool>>>())).ReturnsAsync(entity);

            // دریافت نام صحیح Property مربوط به ریپازیتوری
            var propertyInfo = typeof(IUnitOfWork).GetProperties()
                .FirstOrDefault(p => p.PropertyType == typeof(IRepository<T>));

            if (propertyInfo != null)
            {
                mockUnitOfWork.Setup(u => propertyInfo.GetValue(u)).Returns(repoMock.Object);
            }
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
            var jsonObject = JsonConvert.DeserializeObject<ResponseStatus>(JsonConvert.SerializeObject(badRequestResult.Value));
            Assert.Equal(expectedMessage, jsonObject.StatusMessage?.ToString());
        }

        public static void AssertOkWithMessage(IActionResult result, string expectedMessage)
        {
            var badRequestResult = Assert.IsType<OkObjectResult>(result);
            var jsonObject = JsonConvert.DeserializeObject<ResponseStatus>(JsonConvert.SerializeObject(badRequestResult.Value));
            Assert.Equal(expectedMessage, jsonObject.StatusMessage?.ToString());
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

        public static List<T> ExtractListFromResultWithKey<T>(OkObjectResult okResult, string key)
        {
            Assert.NotNull(okResult.Value);

            // Convert the response object to a JObject
            var jsonObject = JObject.FromObject(okResult.Value);

            // Extract and deserialize the "result" property
            var dataArray = jsonObject[key]?.ToObject<List<T>>();

            Assert.NotNull(dataArray);
            return dataArray;
        }
    }
}
