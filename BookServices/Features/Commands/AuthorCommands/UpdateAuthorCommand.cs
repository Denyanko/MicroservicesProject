using BookServices.Models;
using MediatR;

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
                var author = await _context.Authors.FindAsync(command.Id);

                if (author == null) return false;

                author.Name = command.Name;
                author.Biography = command.Biography;

                await _context.SaveChangesAsync();

                return true;
            }
        }
    }
}
