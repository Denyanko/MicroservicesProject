using AutoMapper;
using StudentServices.Model;

namespace StudentServices.DTO
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Student, StudentDto>();
            CreateMap<Parent, ParentDto>();
        }
    }
}
