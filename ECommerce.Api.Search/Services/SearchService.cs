using ECommerce.Api.Search.Interfaces;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly ICustomerService customerService;
        public SearchService(IOrderService orderService, IProductService productService, ICustomerService customerService)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.customerService = customerService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var orderResult = await orderService.GetOrdersAsync(customerId);
            var productsResult = await productService.GetProductsAsync();
            var customerResult = await customerService.GetCustomerAsync(customerId);

            if (orderResult.IsSuccess)
            {
                foreach(var order in orderResult.Orders)
                {
                    order.CustomerName = customerResult.IsSuccess ?
                        customerResult.Customers?.Name :
                        "Customer Information is not avaialble";

                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ?
                            productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name :
                            "Product Information is not available";
                    }
                }
                var result = new
                {
                    Orders = orderResult.Orders,
                };
                return (true, result);
            }
            return (true, null);
        }
    }
}
