using BookServices.Models;
using MediatR;

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
                var genre = await _context.Genres.FindAsync(command.Id);

                if (genre == null) return false;

                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();

                return true;
            }
        } 
    }
}
