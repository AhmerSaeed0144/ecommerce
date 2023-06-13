using AutoMapper;
using ECommerce.Api.Products.Database;
using ECommerce.Api.Products.Interfaces;
using Microsoft.EntityFrameworkCore;
using Product = ECommerce.Api.Products.Models.Product;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductProvider
    {
        private readonly ProductsDbContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;
        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.Add(new Database.Product() { Id = 1, Inventory = 10, Name = "KeyBoard", Price = 100 });
                dbContext.Products.Add(new Database.Product() { Id = 2, Inventory = 15, Name = "Mouse", Price = 250 });
                dbContext.Products.Add(new Database.Product() { Id = 3, Inventory = 20, Name = "Head Phone", Price = 500 });
                dbContext.Products.Add(new Database.Product() { Id = 4, Inventory = 25, Name = "LCD", Price = 2500 });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();
                if(products != null && products.Any())
                {
                    var result = mapper.Map<IEnumerable<Database.Product>, IEnumerable<Models.Product>>(products);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.ToString());

            }
        }

        public async Task<(bool IsSuccess, Product Products, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var products = await dbContext.Products.FirstOrDefaultAsync(x=>x.Id == id);
                if (products != null)
                {
                    var result = mapper.Map<Database.Product, Models.Product>(products);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.ToString());

            }
        }
    }
}
