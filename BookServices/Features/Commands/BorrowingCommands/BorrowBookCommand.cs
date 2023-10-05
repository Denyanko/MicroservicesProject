using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Commands.BorrowingCommands
{
    public class BorrowBookCommand : IRequest<Borrowing>
    {
        public int BookId { get; set; }
        public int StudentId { get; set; }

        public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, Borrowing>
        {
            private readonly AppDbContext _context;

            public BorrowBookCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<Borrowing> Handle(BorrowBookCommand command, CancellationToken cancellationToken)
            {
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken)) 
                {
                    try
                    {
                        var latestBorrowing = await _context.Borrowings
                            .Where(b => b.BookId == command.BookId && b.StudentId == command.StudentId)
                            .OrderByDescending(b => b.BorrowDate)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (latestBorrowing != null && latestBorrowing.ReturnedDate == null) return null;

                        var borrow = new Borrowing
                        {
                            BookId = command.BookId,
                            StudentId = command.StudentId,
                            BorrowDate = DateTime.Now,
                        };

                        _context.Borrowings.Add(borrow);
                        await _context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return borrow;
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update exception occured while adding borrow data: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }catch(Exception ex)
                    {
                        Console.WriteLine($"Error occurred while adding borrow data: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                }
            }
        }       
    }
}
