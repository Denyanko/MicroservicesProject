using AutoMapper;
using BookServices.Models;

namespace BookServices.DTO
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.BookGenres.Select(bg => bg.Genre)));
            CreateMap<Genre, GenreDto>();
        }
    }
}
