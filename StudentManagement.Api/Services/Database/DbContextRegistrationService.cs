using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Api.Data;

namespace StudentManagement.Api.Services.Database
{
    public static class DbContextRegistrationService
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<SchoolDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
