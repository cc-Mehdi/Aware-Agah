using Datalayer.Repositories.IRepositories;

namespace Agah.API_s
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

        public static async Task CallCheckPriceInReservedsApi(int userId)
        {

        }
    }

}
