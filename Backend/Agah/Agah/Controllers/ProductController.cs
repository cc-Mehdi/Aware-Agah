using Datalayer.Models;
using Datalayer.Repositories;
using Datalayer.Repositories.IRepositories;
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

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                List<string> list = _unitOfWork.ProductRepository.GetAll().Select(u=> u.Title).ToList();

                return Ok(new {result = list});
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
                var list = _unitOfWork.ProductLogRepository.GetAllByInclude()
                    .OrderByDescending(u=> u.CreatedAt)
                    .GroupBy(u => u.Product_Id)
                    .Select(g => g.First())
                    .Select(u => new Product_ProductLog_ViewModel() 
                    { 
                        Product_Id = u.Product_Id,
                        ProductName = u.Product.Title,
                        Price = u.Price.ToString("N0"),
                        CreateAt = u.CreatedAt.ToString(),
                        Unit = "تومان"
                    }).ToList();

                return Ok(new { result = list });
            }
            catch (Exception ex)
            {
                return BadRequest($"ما با خطای {ex.Message} رو به رو شده ایم 😖");
            }
        }

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
                        var existingProduct = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Title == key);

                        if (existingProduct != null)
                        {
                            return Ok($"{key} exists");
                        }
                        else
                        {
                            // Add new product
                            var newProduct = new Product
                            {
                                Title = key,
                                CreatedAt = DateTime.UtcNow
                            };

                            _unitOfWork.ProductRepository.Add(newProduct);
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
            string url = "https://milli.gold/api/v1/public/milli-price/detail";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

                    if (apiResponse != null && apiResponse.FirstOrDefault().Value != null)
                    {
                        string responseKey = apiResponse.FirstOrDefault().Key;
                        string responseValue = apiResponse.FirstOrDefault().Value;

                        // Get last submited product log
                        var lastProductLog = _unitOfWork.ProductLogRepository.GetAllByFilter(u=> u.Product.Title == responseKey).OrderByDescending(u=> u.CreatedAt).FirstOrDefault();

                        decimal.TryParse(responseValue, out decimal price); // Get current product price

                        if (lastProductLog == null) // if not save any product log for this product
                        {
                            int productId = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Title == responseKey).Id; // Get product id

                            if (productId == 0)
                                return Ok("Not submited any product log for this product");


                            _unitOfWork.ProductLogRepository.Add(new ProductLog()
                            {
                                Product_Id = productId,
                                Price = price,
                                CreatedAt = DateTime.Now
                            });

                            await _unitOfWork.SaveAsync();

                            return Ok($"New product log for product {responseKey} added. In price {responseValue}");
                        }
                        else // if product log for this product was submited
                        {
                            if(lastProductLog.Price != price) // check if price changed after last time
                            {
                                _unitOfWork.ProductLogRepository.Add(new ProductLog()
                                {
                                    Product_Id = lastProductLog.Product_Id,
                                    Price = price,
                                    CreatedAt = DateTime.Now
                                });

                                await _unitOfWork.SaveAsync();

                                return Ok($"product log for product {responseKey} updated. {lastProductLog.Price} => {responseValue}");
                            }

                            return Ok($"No price change for product {responseKey}. Current price is {responseValue}");
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

    }

    // Define a model to map the API response
    public class Product_MilliAPI_Response_ViewModel
    {
        [JsonProperty("price18")]
        public string Price18 { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
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
