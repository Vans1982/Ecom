using AutoMapper;

namespace API.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product,ProductsToReturnDto>();
           
        }
    }
}