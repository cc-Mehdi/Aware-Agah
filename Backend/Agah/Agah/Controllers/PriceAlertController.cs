using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Agah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceAlertController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SetPriceAlert([FromBody] PriceAlertRequest request)
        {
            var userSelectedRange = new PriceAlertRequest
            {
                UserId = request.UserId,
                Product = request.Product,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                CreatedAt = DateTime.UtcNow
            };

            return Ok(new { message = "Price alert set successfully!" });
        }

        [HttpGet("GetProductNames")]
        public async Task<IActionResult> GetProductNames()
        {
            // Create the list
            List<string> items = new List<string>
        {
            "گرم طلای 18 عیار",
            "سکه بهار آزادی"
        };

            // Convert the list to JSON
            string json = JsonConvert.SerializeObject(items);


            return Ok(new { result = json });
        }
    }

    public class PriceAlertRequest
    {
        public int UserId { get; set; }
        public string Product { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
