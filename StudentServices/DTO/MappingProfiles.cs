using AutoMapper;
using StudentServices.Model;

namespace StudentServices.DTO
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.Parent, opt => opt.MapFrom(src => src.Parent));
            CreateMap<Parent, ParentDto>();
        }
    }
}
