using Microsoft.EntityFrameworkCore;

3using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Commands.GenreCommands
{
    public class UpdateGenreCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, bool>
        {
            private readonly AppDbContext _context;

            public UpdateGenreCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UpdateGenreCommand command, CancellationToken cancellationToken)
            { 
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var genre = await _context.Genres.FindAsync(command.Id, cancellationToken);

                        if (genre == null) return false;

                        genre.Name = command.Name;

                        await _context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return true;
                    }
                    catch(DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update exception occured while updating a genre: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }catch(Exception ex)
                    {
                        Console.WriteLine($"Error occurred while updating a genre: {ex.Message}");

                        if (transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                }
                
            }
        }
    }
}
