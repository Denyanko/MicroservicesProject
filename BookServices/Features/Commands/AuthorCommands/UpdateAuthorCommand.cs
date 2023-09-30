using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Commands.AuthorCommands
{
    public class UpdateAuthorCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }

        public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, bool>
        {
            private readonly AppDbContext _context;

            public UpdateAuthorCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
            {
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try 
                    {
                        var author = await _context.Authors.FindAsync(command.Id, cancellationToken);

                        if (author == null) return false;

                        author.Name = command.Name;
                        author.Biography = command.Biography;

                        await _context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return true;
                    }catch(DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update exception occurred while updating an author: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error occurred while updating an author: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                    
                }
                
            }
        }
    }
}
