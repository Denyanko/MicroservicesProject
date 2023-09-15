using AutoMapper;
using BookServices.Models;

namespace BookServices.DTO
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Author, AuthorDto>();
        }
    }
}
