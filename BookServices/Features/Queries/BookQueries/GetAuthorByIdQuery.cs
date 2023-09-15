using AutoMapper;
using BookServices.DTO;
using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Queries.BookQueries
{
    public class GetAuthorByIdQuery : IRequest<AuthorDto>
    {
        public int Id { get; set; }

        public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public GetAuthorByIdQueryHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<AuthorDto> Handle(GetAuthorByIdQuery query, CancellationToken cancellationToken)
            {
                var author = await _context.Authors
                    .Include(a => a.Books)
                    .FirstOrDefaultAsync(a => a.Id == query.Id);

                return _mapper.Map<AuthorDto>(author);
            }
        }
    }
}
