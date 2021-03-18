using AutoMapper;

namespace api.birds.MappingProfiles
{
    public class ModelsToResources : Profile
    {
        public ModelsToResources()
        {
            CreateMap<Models.Bird, Resources.BirdResource>().ReverseMap();
        }
    }
}