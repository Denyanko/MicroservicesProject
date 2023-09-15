using BookServices.Models;
using MediatR;

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
                var genre = await _context.Genres.FindAsync(command.Id);

                if (genre == null) return false;

                genre.Name = command.Name;

                await _context.SaveChangesAsync();

                return true;
            }
        }
    }
}
