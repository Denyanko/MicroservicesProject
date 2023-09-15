using AutoMapper;
using BookServices.DTO;
using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookServices.Features.Queries.BookQueries
{
    public class GetAllBooksQuery : IRequest<List<BookDto>>
    {
        public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookDto>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public GetAllBooksQueryHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<BookDto>> Handle(GetAllBooksQuery query, CancellationToken cancellationToken)
            {
                var books = await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.BookGenres)
                        .ThenInclude(bg => bg.Genre)
                    .ToListAsync();

                return _mapper.Map<List<BookDto>>(books);
            }
        }
    }
}
