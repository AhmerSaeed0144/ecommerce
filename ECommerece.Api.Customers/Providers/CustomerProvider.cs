using AutoMapper;
using ECommerce.Api.Customers.Database;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomerProvider : ICustomerProvider
    {
        private readonly CustomerDbConext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<ICustomerProvider>logger;

        public CustomerProvider(CustomerDbConext dbContext, ILogger<ICustomerProvider> logger, IMapper mapper) 
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.logger = logger;
            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Database.Customer() { Id = 1, Address = "Lahore", Name = "Ahmer" });
                dbContext.Customers.Add(new Database.Customer() { Id = 2, Address = "Karachi", Name = "Munawar" });
                dbContext.Customers.Add(new Database.Customer() { Id = 3, Address = "Multan", Name = "Zohaib" });
                dbContext.Customers.Add(new Database.Customer() { Id = 4, Address = "Faislabad", Name = "Majid" });
                dbContext.Customers.Add(new Database.Customer() { Id = 5, Address = "Islamabad", Name = "Usama" });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Customers, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
                if(customer != null)
                {
                    var result = mapper.Map<Database.Customer, Models.Customer>(customer);
                    return (true, result, null);
                }
                return (true, null, "Not Found");
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                return (true, null, ex.ToString());
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Database.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }
                return (true, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return (true, null, ex.ToString());
            }
        }
    }
}
