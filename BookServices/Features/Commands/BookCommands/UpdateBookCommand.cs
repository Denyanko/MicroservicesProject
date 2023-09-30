using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Commands.BookCommands
{
    public class UpdateBookCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public int AuthorId { get; set; }
        public List<int> GenreIds { get; set; }

        public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
        {
            private readonly AppDbContext _context;

            public UpdateBookCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
            {
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var book = await _context.Books
                            .Include(b => b.BookGenres)
                            .FirstOrDefaultAsync(b => b.Id == command.Id, cancellationToken);

                        if (book == null) return false;

                        book.Title = command.Title;
                        book.PublicationDate = command.PublicationDate;
                        book.AuthorId = command.AuthorId;

                        if (command.GenreIds != null)
                        {
                            _context.BookGenres.RemoveRange(book.BookGenres);

                            var genres = await _context.Genres
                                .Where(g => command.GenreIds.Contains(g.Id))
                                .ToListAsync(cancellationToken);

                            foreach (var genre in genres)
                            {
                                book.BookGenres.Add(new BookGenre { GenreId = genre.Id });
                            }
                        }

                        await _context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return true;
                    }catch(DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update exception occurred while updating a book: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error occurred while updating a book: {ex.Message}");

                        if (transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                }
                
            }
        }
    }
}
