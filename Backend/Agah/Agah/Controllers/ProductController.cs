using Agah.Services;
using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Agah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProductService _productService;

        public ProductController(IUnitOfWork unitOfWork, ProductService productService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
        }

        [Authorize]
        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsync();
                var list = products.Select(u => new { u.Id, u.PersianName, u.IconName }).ToList();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest($"ما با خطای {ex.Message} رو به رو شده ایم 😖");
            }
        }

        [HttpGet("GetProductsLog")]
        public async Task<IActionResult> GetProductsLog()
        {
            try
            {
                var productLogs = await _unitOfWork.ProductLogRepository.GetAllAsync(includeProperties: u => u.Product);

                var list = productLogs
                    .OrderByDescending(u => u.CreatedAt)
                    .GroupBy(u => u.Product_Id)
                    .Select(g => g.First())
                    .Select(u => new Product_ProductLog_ViewModel()
                    {
                        Product_Id = u.Product_Id,
                        ProductName = u.Product?.PersianName ?? "",  // Handle null Product
                        ProductIconName = u.Product?.IconName ?? "", // Handle null Product
                        Price = u.Price.ToString("N0"),
                        CreateAt = u.CreatedAt.ToString(),
                        Unit = "ريال"
                    })
                    .ToList();


                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest($"ما با خطای {ex.Message} رو به رو شده ایم 😖");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("SeedProducts")]
        public async Task<IActionResult> SeedProducts() // this method responsible for adding new products to database. read the api and split the product name and added to Product table in db.
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

                    if (apiResponse != null && apiResponse.FirstOrDefault().Key != null)
                    {
                        string key = apiResponse.FirstOrDefault().Key;

                        // Check if key exists in Product table
                        var existingProduct = await _unitOfWork.ProductRepository.GetFirstOrDefaultAsync(p => p.EnglishName == key);

                        if (existingProduct != null)
                        {
                            return Ok($"{key} exists");
                        }
                        else
                        {
                            // Add new product
                            var newProduct = new Product
                            {
                                PersianName = "",
                                IconName = "",
                                EnglishName = key,
                                CreatedAt = DateTime.UtcNow
                            };

                            await _unitOfWork.ProductRepository.AddAsync(newProduct);
                            await _unitOfWork.SaveAsync();

                            return Ok($"{key} added");
                        }
                    }
                    return BadRequest("Invalid API response");
                }
                catch (Exception ex)
                {
                    return BadRequest($"ما با خطای {ex.Message} رو به رو شده ایم 😖");
                }
            }
        }

        [HttpGet("UpdateProductLog")]
        public async Task<IActionResult> UpdateProductLog() // this method responsible for checking product price. if product changed, add new log and else no changes happend.
        {
            ResponseStatus response = await _productService.UpdateProductLog();
            if (response.StatusCode == 200)
                return Ok(response);
            else
                return BadRequest(response);
        }

    }

    public class Product_ProductLog_ViewModel
    {
        public int Product_Id { get; set; }
        public string ProductName { get; set; }
        public string ProductIconName { get; set; }
        public string Price { get; set; }
        public string Unit { get; set; }
        public string CreateAt { get; set; }
    }
}









// TODO: this code work for list of jsons (in above code we just get "price18" parameter and cant run below code). when we work on multi products, we can run below code and delete above code
//using Datalayer.Models;
//using Datalayer.Repositories;
//using Datalayer.Repositories.IRepositories;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Linq;

//namespace Agah.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProductController : ControllerBase
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public ProductController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        [HttpGet]
//        public async Task<IActionResult> SeedProducts()
//        {
//            string url = "https://milli.gold/api/v1/public/milli-price/detail";

//            using (HttpClient httpClient = new HttpClient())
//            {
//                try
//                {
//                    HttpResponseMessage response = await httpClient.GetAsync(url);
//                    response.EnsureSuccessStatusCode();

//                    string responseBody = await response.Content.ReadAsStringAsync();

//                    // Deserialize JSON into a list of dictionaries
//                    var jsonDataList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(responseBody);

//                    if (jsonDataList == null || jsonDataList.Count == 0)
//                    {
//                        return BadRequest("Invalid API response");
//                    }

//                    List<string> results = new List<string>();

//                    foreach (var jsonRecord in jsonDataList)
//                    {
//                        if (jsonRecord.Count > 0)
//                        {
//                            // Extract only the first key
//                            string firstKey = jsonRecord.Keys.First();

//                            // Check if the key exists in the Product table
//                            var existingProduct = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Title == firstKey);

//                            if (existingProduct != null)
//                            {
//                                results.Add($"{firstKey} exists");
//                            }
//                            else
//                            {
//                                // Add new product with the key as the Title
//                                var newProduct = new Product
//                                {
//                                    Title = firstKey,
//                                    CreatedAt = DateTime.UtcNow
//                                };

//                                _unitOfWork.ProductRepository.Add(newProduct);
//                                await _unitOfWork.SaveAsync();

//                                results.Add($"{firstKey} added");
//                            }
//                        }
//                    }

//                    return Ok(results);
//                }
//                catch (Exception ex)
//                {
//                    return BadRequest($"Error: {ex.Message}");
//                }
//            }
//        }
//    }
//}
