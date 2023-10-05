using BookServices.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var student = new Student
                    {
                        Id = consumeContext.Message.Id,
                        Name = consumeContext.Message.Name,
                    };
                    
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    await consumeContext.ConsumeCompleted;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error occurred while consuming student created message");

                    if(transaction != null) transaction.Rollback();

                    throw;
                }
            }
        }
    }
}
