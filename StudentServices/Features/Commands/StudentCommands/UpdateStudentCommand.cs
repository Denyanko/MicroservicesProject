using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Model;
using StudentServices.Model;

namespace StudentServices.Features.Commands.StudentCommands
{
    public class UpdateStudentCommand : IRequest<bool>
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentAddress { get; set; }
        public string StudentGender { get; set; }
        public DateTime StudentDOB { get; set; }
        public string FatherName { get; set; }
        public string FatherAddress { get; set; }
        public string FatherPhone { get; set; }
        public string MotherName { get; set; }
        public string MotherAddress { get; set; }
        public string MotherPhone { get; set; }

        public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, bool>
        {
            private readonly AppDbContext _context;
            private readonly IPublishEndpoint _publishEndpoint;

            public UpdateStudentCommandHandler(AppDbContext context, IPublishEndpoint publishEndpoint)
            {
                _context = context;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<bool> Handle(UpdateStudentCommand command, CancellationToken cancellationToken)
            {
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken)) 
                {
                    try
                    {
                        var student = await _context.Students
                            .Include(s => s.Parent)
                            .FirstOrDefaultAsync(s => s.Id == command.StudentId, cancellationToken);

                        if (student == null) return false;

                        student.Name = command.StudentName;
                        student.Address = command.StudentAddress;
                        student.Gender = command.StudentGender;
                        student.DOB = command.StudentDOB;

                        if (student.Parent == null) student.Parent = new Parent();

                        student.Parent.FatherName = command.FatherName;
                        student.Parent.FatherAddress = command.FatherAddress;
                        student.Parent.FatherPhone = command.FatherPhone;
                        student.Parent.MotherName = command.MotherName;
                        student.Parent.MotherAddress = command.MotherAddress;
                        student.Parent.MotherPhone = command.MotherPhone;

                        await _context.SaveChangesAsync(cancellationToken);

                        await _publishEndpoint.Publish(new StudentUpdated
                        {
                            Id = student.Id,
                            Name = student.Name,
                        }, cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return true;
                    }
                    catch(DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update exception occurred while updating a student: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }catch (Exception ex) 
                    {
                        Console.Write($"Error occurred while updating a student: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                }
                
            }
        }
    }
}
