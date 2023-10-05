using BookServices.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Model;

namespace BookServices.Consumer
{
    public class StudentUpdatedConsumer : IConsumer<StudentUpdated>
    {
        private readonly AppDbContext _context;

        public StudentUpdatedConsumer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<StudentUpdated> consumeContext)
        {
            var student = await _context.Students
                    .Where(s => s.Id == consumeContext.Message.Id)
                    .FirstAsync();

            if(student != null)
            {
                student.Name = consumeContext.Message.Name;
            }

            await _context.SaveChangesAsync();

            await consumeContext.ConsumeCompleted;
        }
    }
}
