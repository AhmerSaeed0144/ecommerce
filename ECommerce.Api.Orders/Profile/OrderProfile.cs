namespace ECommerce.Api.Orders.Profile
{
    public class OrderProfile : AutoMapper.Profile
    {
        public OrderProfile() 
        {
            CreateMap<Database.Order, Models.Order>();
            CreateMap<Database.OrderItem, Models.OrderItem>();
        }
    }
}
