namespace ECommerce.Api.Customers.Profile
{
    public class CustomerProfile : AutoMapper.Profile
    {
        public CustomerProfile()
        {
            CreateMap<Database.Customer, Models.Customer>();
        }
    }
}
