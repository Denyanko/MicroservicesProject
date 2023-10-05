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
                .Where(s => s.Id == consumeContext.Message.Id)
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
