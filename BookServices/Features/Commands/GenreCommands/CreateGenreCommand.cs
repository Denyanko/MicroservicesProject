using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Commands.GenreCommands
{
    public class CreateGenreCommand : IRequest<int>
    {
        public string Name { get; set; }

        public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, int>
        {
            private readonly AppDbContext _context;

            public CreateGenreCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateGenreCommand command, CancellationToken cancellationToken)
            {
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var genre = new Genre { Name = command.Name };

                        _context.Genres.Add(genre);
                        await _context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return genre.Id;
                    }catch(DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update exception occurred while creating a genre: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error occured while creating a genre: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                }
                
            }
        }
    }
}
