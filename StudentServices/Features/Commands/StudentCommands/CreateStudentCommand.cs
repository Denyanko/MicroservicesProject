using MediatR;
using StudentServices.Model;

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

            public CreateStudentCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateStudentCommand command, CancellationToken cancellationToken)
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

                return student.Id;
            }
        }
    }
}
