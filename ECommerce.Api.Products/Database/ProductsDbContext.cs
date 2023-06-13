using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Products.Database
{
    public class ProductsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set;}

        public ProductsDbContext(DbContextOptions option) : base(option) 
        {

        }
    }
}
