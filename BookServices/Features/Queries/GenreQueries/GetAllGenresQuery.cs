using AutoMapper;
using BookServices.DTO;
using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Queries.GenreQueries
{
    public class GetAllGenresQuery : IRequest<List<GenreDto>>
    {
        public class GetAllGenresQueryHandler : IRequestHandler<GetAllGenresQuery, List<GenreDto>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public GetAllGenresQueryHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<GenreDto>> Handle(GetAllGenresQuery query, CancellationToken cancellationToken)
            {
                var genres = await _context.Genres
                    .Include(g => g.BookGenres)
                        .ThenInclude(bg => bg.Book)
                    .ToListAsync();

                return _mapper.Map<List<GenreDto>>(genres);
            }
        }
    }
}
