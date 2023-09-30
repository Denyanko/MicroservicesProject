using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Commands.BookCommands
{
    public class DeleteBookCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, bool>
        {
            private readonly AppDbContext _context;

            public DeleteBookCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
            {
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var book = await _context.Books.FindAsync(command.Id, cancellationToken);

                        if (book == null) return false;

                        _context.Books.Remove(book);
                        await _context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return true;
                    }catch(DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update exception occurred while deleting a book: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error occurred while deleting a book: {ex.Message}");

                        if(transaction!= null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                }
            }
        }
    }
}
