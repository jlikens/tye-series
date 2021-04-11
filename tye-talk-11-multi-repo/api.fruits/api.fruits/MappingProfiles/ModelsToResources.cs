using AutoMapper;

namespace api.fruits.MappingProfiles
{
    public class ModelsToResources : Profile
    {
        public ModelsToResources()
        {
            CreateMap<Models.Fruit, Resources.FruitResource>().ReverseMap();
        }
    }
}