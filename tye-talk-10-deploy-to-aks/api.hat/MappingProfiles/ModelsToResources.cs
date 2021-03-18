using AutoMapper;

namespace api.hat.MappingProfiles
{
    public class ModelsToResources : Profile
    {
        public ModelsToResources()
        {
            CreateMap<Models.Hat, Resources.HatResource>().ReverseMap();
        }
    }
}