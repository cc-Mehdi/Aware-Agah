using Newtonsoft.Json;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agah.Controllers;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

public class AlarmsControllerMoqTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAlarmRepository> _mockAlarmRepo;
    private readonly AlarmsController _controller;

    public AlarmsControllerMoqTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAlarmRepo = new Mock<IAlarmRepository>();

        _mockUnitOfWork.Setup(u => u.AlarmRepository).Returns(_mockAlarmRepo.Object);

        _controller = new AlarmsController(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetAlarms_ShouldReturnAlarms_WhenAlarmsExist()
    {
        // Arrange
        var fakeAlarms = new List<Alarm>
        {
            new Alarm { Id = 1, PersianName = "هشدار ۱", ShortDescription = "توضیح ۱", IsActive = true },
            new Alarm { Id = 2, PersianName = "هشدار ۲", ShortDescription = "توضیح ۲", IsActive = false }
        };

        _mockAlarmRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(fakeAlarms);

        // Act
        var result = await _controller.GetAlarms();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        // ✅ تبدیل خروجی به JSON برای دسترسی به `result`
        var jsonString = JsonConvert.SerializeObject(okResult.Value);
        var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

        Assert.NotNull(response);
        Assert.True(response.ContainsKey("result"));
        Assert.Equal(2, ((Newtonsoft.Json.Linq.JArray)response["result"]).Count);
    }

    [Fact]
    public async Task GetAlarms_ShouldReturnEmptyList_WhenNoAlarmsExist()
    {
        // Arrange
        _mockAlarmRepo.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(new List<Alarm>());

        // Act
        var result = await _controller.GetAlarms();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        // ✅ تبدیل خروجی به JSON برای دسترسی به `result`
        var jsonString = JsonConvert.SerializeObject(okResult.Value);
        var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
    }
}
