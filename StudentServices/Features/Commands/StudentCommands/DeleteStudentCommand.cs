using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Model;
using StudentServices.Model;

namespace StudentServices.Features.Commands.StudentCommands
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public int StudentId { get; set; }

        public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
        {
            private readonly AppDbContext _context;
            private readonly IPublishEndpoint _publishEndpoint;

            public DeleteStudentCommandHandler(AppDbContext context, IPublishEndpoint publishEndpoint)
            {
                _context = context;
                _publishEndpoint = publishEndpoint;
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

                        await _publishEndpoint.Publish(new StudentDeleted
                        {
                            StudentId = student.Id,
                        }, cancellationToken);

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
