using BookServices.Models;
using MediatR;

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
                var author = new Author
                {
                    Name = command.Name,
                    Biography = command.Biography
                };

                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                return author.Id;
            }
        }
    }
}
