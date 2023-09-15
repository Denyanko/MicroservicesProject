using MediatR;
using StudentServices.Model;

namespace StudentServices.Features.Commands.StudentCommands
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public int StudentId { get; set; }

        public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
        {
            private readonly AppDbContext _context;

            public DeleteStudentCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeleteStudentCommand command, CancellationToken cancellationToken)
            {
                var student = await _context.Students.FindAsync(command.StudentId, cancellationToken);
                
                if (student == null) return false;

                _context.Students.Remove(student);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
