using AutoMapper;
using ECommerce.Api.Orders.Database;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrderDbContext orderDbContext;
        private readonly ILogger<IOrdersProvider> logger;
        private readonly IMapper mapper;
        public OrdersProvider(OrderDbContext orderDbContext, ILogger<IOrdersProvider> logger, IMapper mapper) 
        {
            this.orderDbContext = orderDbContext;
            this.mapper = mapper;
            this.logger = logger;

            SeedData();
        }

        private void SeedData()
        {
            if(!orderDbContext.Orders.Any())
            {
                orderDbContext.Orders.Add(new Database.Order()
                {
                    CustomerId = 1,
                    Id = 1,
                    OrderDate = System.DateTime.Now,
                    Total = 500,
                    Items = new List<Database.OrderItem>()
                    {
                        new Database.OrderItem(){
                            Id= 1,
                            OrderId=1,
                            ProductId=1,
                            Quantity=3,
                            UnitPrice=25
                        }
                    }
                });

                orderDbContext.Orders.Add(new Database.Order()
                {
                    CustomerId = 2,
                    Id = 2,
                    OrderDate = System.DateTime.Now,
                    Total = 1500,
                    Items = new List<Database.OrderItem>()
                    {
                        new Database.OrderItem(){
                            Id= 2,
                            OrderId=2,
                            ProductId=2,
                            Quantity=5,
                            UnitPrice=205
                        }
                    }
                });

                orderDbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrderAsync(int customerId)
        {
            try 
            {
                var orders = await orderDbContext.Orders.Where(x=>x.CustomerId == customerId).ToListAsync();
                if(orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Database.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex) 
            {
                logger.LogError(ex.Message);
                return (false, null, ex.Message);
            }
        }
    }
}
