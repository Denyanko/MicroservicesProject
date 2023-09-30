using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Commands.BookCommands
{
    public class CreateBookCommand : IRequest<int>
    {
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public int AuthorId { get; set; }
        public List<int> GenreIds { get; set; }

        public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, int>
        {
            private readonly AppDbContext _context;

            public CreateBookCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateBookCommand command, CancellationToken cancellationToken)
            {
                using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var book = new Book
                        {
                            Title = command.Title,
                            PublicationDate = command.PublicationDate,
                            AuthorId = command.AuthorId,
                        };

                        if (command.GenreIds != null)
                        {
                            var genres = await _context.Genres
                                .Where(g => command.GenreIds.Contains(g.Id))
                                .ToListAsync(cancellationToken);

                            if (book.BookGenres == null)
                            {
                                book.BookGenres = new List<BookGenre>();
                            }

                            foreach (var genre in genres)
                            {
                                book.BookGenres.Add(new BookGenre { GenreId = genre.Id });
                            }
                        }

                        _context.Books.Add(book);
                        await _context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return book.Id;
                    }
                    catch(Exception ex) 
                    {
                        Console.WriteLine($"Error occurred while creating a book: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                }
            }
        }
    }
}
