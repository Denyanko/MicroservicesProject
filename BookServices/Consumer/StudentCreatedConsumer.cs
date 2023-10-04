using BookServices.Models;
using MassTransit;
using Shared.Model;

namespace BookServices.Consumer
{
    public class StudentCreatedConsumer : IConsumer<StudentCreated>
    {
        private readonly AppDbContext _context;

        public StudentCreatedConsumer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<StudentCreated> consumeContext)
        {
            var student = new Student
            {
                StudentId = consumeContext.Message.StudentId,
                Name = consumeContext.Message.Name,
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            await consumeContext.ConsumeCompleted;
        }
    }
}
