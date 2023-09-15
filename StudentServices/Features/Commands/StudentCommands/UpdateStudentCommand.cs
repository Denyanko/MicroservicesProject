﻿using MediatR;
using Microsoft.EntityFrameworkCore;
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

            public UpdateStudentCommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UpdateStudentCommand command, CancellationToken cancellationToken)
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

                return true;
            }
        }
    }
}