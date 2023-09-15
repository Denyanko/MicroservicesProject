using AutoMapper;
using BookServices.DTO;
using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Queries.GenreQueries
{
    public class GetGenreByIdQuery : IRequest<GenreDto>
    {
        public int Id { get; set; }

        public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, GenreDto>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public GetGenreByIdQueryHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<GenreDto> Handle(GetGenreByIdQuery query, CancellationToken cancellationToken)
            {
                var genre = await _context.Genres
                    .Include(g => g.BookGenres)
                        .ThenInclude(bg => bg.Book)
                    .FirstOrDefaultAsync(g => g.Id == query.Id);

                return _mapper.Map<GenreDto>(genre);
            }
        }
    }
}
