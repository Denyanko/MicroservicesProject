using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentServices.DTO;
using StudentServices.Model;

namespace StudentServices.Features.Queries
{
    public class GetStudentByIdQuery : IRequest<StudentDto>
    {
        public int StudentId { get; set; }

        public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDto>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public GetStudentByIdQueryHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<StudentDto> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken)
            {
                var student = await _context.Students
                    .Include(s => s.Parent)
                    .FirstOrDefaultAsync(s => s.Id == query.StudentId, cancellationToken);

                return _mapper.Map<StudentDto>(student);
            }
            
        }
    }
}
