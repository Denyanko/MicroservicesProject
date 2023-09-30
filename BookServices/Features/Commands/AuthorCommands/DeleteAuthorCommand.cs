using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Commands.AuthorCommands
{
    public class DeleteAuthorCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
        {
            private readonly AppDbContext _context;

            public DeleteAuthorCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
            {
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var author = await _context.Authors.FindAsync(command.Id, cancellationToken);

                        if (author == null) return false;

                        _context.Authors.Remove(author);
                        await _context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return true;
                    }catch(DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update exception occurred while deleting an author: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error occurred while deleting an author: {ex.Message}");

                        if(transaction!= null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                    
                }

                
            }
        }
    }
}
