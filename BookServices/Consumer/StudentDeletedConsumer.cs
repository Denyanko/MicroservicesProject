using BookServices.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Model;

namespace BookServices.Consumer
{
    public class StudentDeletedConsumer : IConsumer<StudentDeleted>
    {
        private readonly AppDbContext _context;

        public StudentDeletedConsumer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<StudentDeleted> consumeContext)
        {
            var student = await _context.Students
                .Where(s => s.StudentId == consumeContext.Message.StudentId)
                .FirstAsync();

            if(student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();

            await consumeContext.ConsumeCompleted;
        }
    }
}
