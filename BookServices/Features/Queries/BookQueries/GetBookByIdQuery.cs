using AutoMapper;
using BookServices.DTO;
using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Queries.BookQueries
{
    public class GetBookByIdQuery : IRequest<BookDto>
    {
        public int Id { get; set; }

        public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public GetBookByIdQueryHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<BookDto> Handle(GetBookByIdQuery query, CancellationToken cancellationToken)
            {
                var book = await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.BookGenres)
                        .ThenInclude(bg => bg.Genre)
                    .FirstOrDefaultAsync(b => b.Id == query.Id, cancellationToken);

                return _mapper.Map<BookDto>(book);
            }
        }
    }
}
