using MediatR;
using Microsoft.EntityFrameworkCore;
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
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var student = await _context.Students.FindAsync(command.StudentId, cancellationToken);

                        if (student == null) return false;

                        _context.Students.Remove(student);
                        await _context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return true;
                    }
                    catch(DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update exception occurred while deleting a student: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error occurred while deleting a student: {ex.Message}");

                        if (transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                }
                
            }
        }
    }
}
