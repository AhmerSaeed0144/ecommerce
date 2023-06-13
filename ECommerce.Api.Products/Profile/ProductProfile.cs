namespace ECommerce.Api.Products.Profile
{
    public class ProductProfile: AutoMapper.Profile
    {
        public ProductProfile()
        {
            CreateMap<Database.Product, Models.Product>();
        }
    }
}
