using BookServices.Models;
using MediatR;

namespace BookServices.Features.Commands.BookCommands
{
    public class DeleteBookCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, bool>
        {
            private readonly AppDbContext _context;

            public DeleteBookCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
            {
                var book = await _context.Books.FindAsync(command.Id);

                if (book == null) return false;

                _context.Books.Remove(book);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
