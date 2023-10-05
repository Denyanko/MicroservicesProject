using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Commands.BorrowingCommands
{
    public class ReturnBookCommand : IRequest<bool>
    {
        public int BookId { get; set; }
        public int StudentId { get; set; }

        public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, bool>
        {
            private readonly AppDbContext _context;

            public ReturnBookCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(ReturnBookCommand command, CancellationToken cancellationToken)
            {
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken)) 
                {
                    try
                    {
                        var borrowing = await _context.Borrowings
                            .Where(b => b.StudentId == command.StudentId && b.BookId == command.BookId)
                            .OrderByDescending(b => b.BorrowDate)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (borrowing == null) return false;

                        borrowing.ReturnedDate = DateTime.Now;

                        await _context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return true;

                    }catch(DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update exception occurred while updating borrowing table: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }catch(Exception ex)
                    {
                        Console.WriteLine($"Error occurred while updating borrowing table: {ex.Message}");

                        if (transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                }
            }
        }
    }
}
