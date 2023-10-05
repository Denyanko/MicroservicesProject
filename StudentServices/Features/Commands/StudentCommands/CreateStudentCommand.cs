using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentServices.Model;
using Shared.Model;

namespace StudentServices.Features.Commands.StudentCommands
{
    public class CreateStudentCommand : IRequest<int>
    {
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

        public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, int>
        {
            private readonly AppDbContext _context;
            private readonly IPublishEndpoint _publishEndpoint;

            public CreateStudentCommandHandler(AppDbContext context, IPublishEndpoint publishEndpoint)
            {
                _context = context;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<int> Handle(CreateStudentCommand command, CancellationToken cancellationToken)
            {
                await using(var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var parent = new Parent
                        {
                            FatherName = command.FatherName,
                            FatherAddress = command.FatherAddress,
                            FatherPhone = command.FatherPhone,
                            MotherName = command.MotherName,
                            MotherAddress = command.MotherAddress,
                            MotherPhone = command.MotherPhone,
                        };

                        var student = new Student
                        {
                            Name = command.StudentName,
                            Address = command.StudentAddress,
                            Gender = command.StudentGender,
                            DOB = command.StudentDOB,
                            Parent = parent,
                        };

                        _context.Students.Add(student);
                        await _context.SaveChangesAsync(cancellationToken);

                        await _publishEndpoint.Publish(new StudentCreated
                        {
                            Id = student.Id,
                            Name = student.Name,
                        }, cancellationToken);

                        await transaction.CommitAsync(cancellationToken);

                        return student.Id;
                    }
                    catch(DbUpdateException ex)
                    {
                        Console.WriteLine($"Database update exception occured while creating a student: {ex.Message}");

                        if(transaction != null) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }catch(Exception ex)
                    {
                        Console.WriteLine($"Error occurred while creating a student: {ex.Message}");

                        if(transaction != null ) await transaction.RollbackAsync(cancellationToken);

                        throw;
                    }
                }
            }
        }
    }
}
