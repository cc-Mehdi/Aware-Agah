using Agah.ViewModels;

namespace Agah.Services
{
    public class ApiJobService
    {
        private readonly ProductService _productService;
        private readonly ReserveService _reserveService;

        public ApiJobService(ProductService productService, ReserveService reserveService)
        {
            _productService = productService;
            _reserveService = reserveService;
        }

        public async Task CallUpdateProductLogApi()
        {
            ResponseStatus result = await _productService.UpdateProductLog();
            Console.WriteLine($"Code : {result.StatusCode} | Message : {result.StatusMessage}");
        }

        public async Task CallCheckPriceInReservedsApi()
        {
            var reservesList = await _reserveService.GetReserves();
            if (reservesList == null)
            {
                Console.WriteLine("No data found in GetReserves service calling");
                return;
            }

            foreach (var reserve in reservesList)
            {
                var result = await _reserveService.CheckPriceInReserveds(reserve.User_Id);
                Console.WriteLine($"Checked price for User {reserve.User_Id}: {result}");
            }
        }
    }
}