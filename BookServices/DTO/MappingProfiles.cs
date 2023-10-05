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
            CreateMap<Student, StudentDto>();
            CreateMap<Borrowing, BorrowingDto>()
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book))
                .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Student));
        }
    }
}
