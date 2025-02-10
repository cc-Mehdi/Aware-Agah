using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Newtonsoft.Json;

namespace Agah.Services
{
    public static class ApiJobService
    {
        public static async Task CallUpdateProductLogApi()
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://localhost:44314/api/Product/UpdateProductLog";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"UpdateProductLog API Response: {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error calling UpdateProductLog API: {ex.Message}");
                }
            }
        }

        public static async Task CallCheckPriceInReservedsApi()
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://localhost:44314/api/PriceAlert/GetReserves";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    var reservesList = JsonConvert.DeserializeObject<List<Reserve>>(result);

                    string checkPriceUrl = "";


                    foreach (var reserve in reservesList)
                    {
                        checkPriceUrl = $"https://localhost:44314/api/PriceAlert/CheckPriceInReserveds/{reserve.User_Id}";
                        response = await client.GetAsync(checkPriceUrl);
                        response.EnsureSuccessStatusCode();

                        result = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Checked price for User {reserve.User_Id}: {result}");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error calling CheckPriceInReserveds API: {ex.Message}");
                }
            }
        }
    }

}
