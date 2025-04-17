using Agah.Controllers;
using Agah.Services;
using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace TestLayer
{
    public class ProductControllerMoqTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IProductLogRepository> _mockProductLogsRepository;
        private readonly ProductController _controller;

        public ProductControllerMoqTest(ProductService productService)
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockProductLogsRepository = new Mock<IProductLogRepository>();

            _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepository.Object);
            _mockUnitOfWork.Setup(u => u.ProductLogRepository).Returns(_mockProductLogsRepository.Object);

            _controller = new ProductController(_mockUnitOfWork.Object, productService);
        }

        // =========> GetProductsLog Endpoint
        [Fact]
        public async Task GetProductsLog_ReturnsOkResult_WithProductList()
        {
            // Arrange
            var fakeProducts = FakeDataGenerator.GenerateProducts(1); // Generate fake products
            var fakeProductLogs = FakeDataGenerator.GenerateProductLogs(2, fakeProducts); // Generate fake product logs

            // Mock the repository to return the fake product logs
            _mockProductLogsRepository
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<Expression<Func<ProductLog, object>>[]>() // Mock includeProperties
                ))
                .ReturnsAsync(fakeProductLogs);

            // Mock the UnitOfWork to return the mocked repository
            _mockUnitOfWork
                .Setup(u => u.ProductLogRepository)
                .Returns(_mockProductLogsRepository.Object);

            // Act
            var result = await _controller.GetProductsLog();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Extract the "result" property from the response
            var productLogsList = TestHelper.ExtractListFromResultWithKey<Product_ProductLog_ViewModel>(okResult, "result");

            // Verify the response
            Assert.NotNull(productLogsList);
            Assert.IsType<List<Product_ProductLog_ViewModel>>(productLogsList);

            // Verify the count of returned logs matches the expected count
            Assert.Equal(fakeProductLogs.GroupBy(u => u.Product_Id).Count(), productLogsList.Count);

            // Verify the first item's properties
            var firstLog = productLogsList.First();
            var expectedLog = fakeProductLogs
                .OrderByDescending(u => u.CreatedAt)
                .GroupBy(u => u.Product_Id)
                .Select(g => g.First())
                .First();

            Assert.Equal(expectedLog.Product_Id, firstLog.Product_Id);
            Assert.Equal(expectedLog.Product?.PersianName ?? "", firstLog.ProductName);
            Assert.Equal(expectedLog.Product?.IconName ?? "", firstLog.ProductIconName);
            Assert.Equal(expectedLog.Price.ToString("N0"), firstLog.Price);
            Assert.Equal(expectedLog.CreatedAt.ToString(), firstLog.CreateAt);
            Assert.Equal("ريال", firstLog.Unit);
        }

        [Fact]
        public async Task GetProductsLog_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            _mockProductLogsRepository
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<Expression<Func<ProductLog, object>>[]>()
                ))
                .ThrowsAsync(new System.Exception("Test exception"));

            _mockUnitOfWork
                .Setup(u => u.ProductLogRepository)
                .Returns(_mockProductLogsRepository.Object);

            // Act
            var result = await _controller.GetProductsLog();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ما با خطای Test exception رو به رو شده ایم 😖", badRequestResult.Value);
        }
        // =========> End GetProductsLog Endpoint

        // =========> GetProducts Endpoint
        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithProductList()
        {
            // Arrange
            var fakeProducts = FakeDataGenerator.GenerateProducts(2);

            _mockProductRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(fakeProducts);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // ✅ Extract the correct "result" property
            var productsList = TestHelper.ExtractListFromResultWithKey<ProductDto>(okResult, "result");

            Assert.NotNull(productsList);
            Assert.IsType<List<ProductDto>>(productsList); // ✅ Corrected assertion
            Assert.Equal(fakeProducts.Count, productsList.Count);

            Assert.Equal(fakeProducts[0].Id, productsList[0].Id);
            Assert.Equal(fakeProducts[0].PersianName, productsList[0].PersianName);
            Assert.Equal(fakeProducts[0].IconName, productsList[0].IconName);
        }

        [Fact]
        public async Task GetProducts_ReturnsBadRequest_WhenExceptionOccurs()
        {
            _mockProductRepository
                .Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ما با خطای Test exception رو به رو شده ایم 😖", badRequestResult.Value);
        }
        // =========> End GetProducts Endpoint

        public class ProductDto
        {
            public int Id { get; set; }
            public string PersianName { get; set; }
            public string IconName { get; set; }
        }
    }
}
