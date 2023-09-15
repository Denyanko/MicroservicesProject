using BookServices.Models;
using MediatR;

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
                var genre = new Genre { Name = command.Name };

                _context.Genres.Add(genre);
                await _context.SaveChangesAsync(cancellationToken);

                return genre.Id;
            }
        }
    }
}
