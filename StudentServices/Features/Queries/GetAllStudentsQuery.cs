using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentServices.DTO;
using StudentServices.Model;

namespace StudentServices.Features.Queries
{
    public class GetAllStudentsQuery : IRequest<List<StudentDto>>
    {
        public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, List<StudentDto>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public GetAllStudentsQueryHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<StudentDto>> Handle(GetAllStudentsQuery query, CancellationToken cancellationToken)
            {
                var students = await _context.Students
                    .Include(s => s.Parent)
                    .ToListAsync(cancellationToken);

                return _mapper.Map<List<StudentDto>>(students);
            }
        }
    }
}
