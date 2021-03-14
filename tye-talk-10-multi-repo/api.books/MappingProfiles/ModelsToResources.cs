using AutoMapper;

namespace api.books.MappingProfiles
{
    public class ModelsToResources : Profile
    {
        public ModelsToResources()
        {
            CreateMap<Models.Person, Resources.PersonResource>().ReverseMap();
            CreateMap<Models.Book, Resources.BookResource>().ReverseMap();
        }
    }
}