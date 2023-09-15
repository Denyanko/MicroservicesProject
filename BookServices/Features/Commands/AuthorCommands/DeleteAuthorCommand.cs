using BookServices.Models;
using MediatR;

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
                var author = await _context.Authors.FindAsync(command.Id);

                if (author == null) return false;

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
