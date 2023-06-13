using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Customers.Database
{
    public class CustomerDbConext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public CustomerDbConext(DbContextOptions option) : base(option)
        {

        }
    }
}
