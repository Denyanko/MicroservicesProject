using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Commands.AuthorCommands
{
    public class CreateAuthorCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Biography { get; set; }

        public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, int>
        {
            private readonly AppDbContext _context;

            public CreateAuthorCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
            {
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var author = new Author
                        {
                            Name = command.Name,
                            Biography = command.Biography,
                        };

                        _context.Authors.Add(author);
                        await _context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return author.Id;
                    }
                    catch(DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update occurred while creating an author: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error occurred while creating an author: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                }
            }
        }
    }
}
