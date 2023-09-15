using AutoMapper;
using BookServices.DTO;
using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Queries.BookQueries
{
    public class GetAllAuthorsQuery : IRequest<List<AuthorDto>>
    {
        public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, List<AuthorDto>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public GetAllAuthorsQueryHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<AuthorDto>> Handle(GetAllAuthorsQuery query, CancellationToken cancellationToken)
            {
                var authors = await _context.Authors
                    .Include(a => a.Books)
                    .ToListAsync();

                return _mapper.Map<List<AuthorDto>>(authors);
            }
        }
    }
}
