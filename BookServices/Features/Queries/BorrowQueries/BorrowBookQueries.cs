using AutoMapper;
using BookServices.DTO;
using BookServices.Features.Queries.BookQueries;
using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Queries.BorrowQueries
{
    public class BorrowBookQueries : IRequest<List<BorrowingDto>>
    {
        public int? BookId { get; set; }
        public int? StudentId { get; set; }

        public class BorrowBookQueriesHandler : IRequestHandler<BorrowBookQueries, List<BorrowingDto>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public BorrowBookQueriesHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<BorrowingDto>> Handle(BorrowBookQueries query, CancellationToken cancellationToken)
            {
                var q = _context.Borrowings
                    .Include(b => b.Student)
                    .Include(b => b.Book)
                        .ThenInclude(bk => bk.Author)
                    .AsQueryable();

                if(query.BookId.HasValue) q = q.Where(b => b.BookId == query.BookId);

                if(query.StudentId.HasValue) q = q.Where(b => b.StudentId == query.StudentId);

                var borrowings = await q.ToListAsync(cancellationToken);

                return _mapper.Map<List<BorrowingDto>>(borrowings);
            }
        }
    }
}
