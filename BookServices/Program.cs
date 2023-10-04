using BookServices.Consumer;
using BookServices.DTO;
using BookServices.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace BookServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.AddMassTransit(options =>
            {
                options.AddConsumer<StudentCreatedConsumer>();
                options.AddConsumer<StudentUpdatedConsumer>();
                options.AddConsumer<StudentDeletedConsumer>();

                options.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost:4001"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("event-listener", e =>
                    {
                        e.ConfigureConsumer<StudentCreatedConsumer>(context);
                        e.ConfigureConsumer<StudentUpdatedConsumer>(context);
                        e.ConfigureConsumer<StudentDeletedConsumer>(context);
                    });
                });
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}