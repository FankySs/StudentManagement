using StudentManagement.Api.Services.Auth;

namespace StudentManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // JWT Authentication
            builder.Services.AddJwtAuthentication(builder.Configuration);

            var app = builder.Build();

            // Middleware pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // musí být pøed Authorization
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
