using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Api.Data;
using StudentManagement.Api.Services.Auth;
using StudentManagement.Api.Services.Database;

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

            // Vlastní služby (JWT + DbContext)
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddAppDbContext(builder.Configuration);

            var app = builder.Build();

            // Seeder dat (admin úèet atd.)
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
                DbSeeder.Seed(db, builder.Configuration);
            }

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
