using BookServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookServices.Features.Commands.GenreCommands
{
    public class DeleteGenreCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, bool>
        {
            private readonly AppDbContext _context;

            public DeleteGenreCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeleteGenreCommand command, CancellationToken cancellationToken)
            {
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var genre = await _context.Genres.FindAsync(command.Id, cancellationToken);

                        if (genre == null) return false;

                        _context.Genres.Remove(genre);
                        await _context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return true;
                    }catch(DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update exception occurred while deleting a genre: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error occurred while deleting a genre: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                }
                
            }
        } 
    }
}
