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
    public class ProductControllerMoqTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductController _controller;

        public ProductControllerMoqTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();

            _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepository.Object);

            _controller = new ProductController(_mockUnitOfWork.Object);
        }

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
        // =========> End GetProducts Endpoint

        public class ProductDto
        {
            public int Id { get; set; }
            public string PersianName { get; set; }
            public string IconName { get; set; }
        }
    }
}
