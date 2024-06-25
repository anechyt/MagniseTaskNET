using MagniseTaskNET.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MagniseTaskNET.WebApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static async Task<IServiceCollection> AddDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration["Db:DefaultConnectionString"];
            services.AddDbContext<MagniseTaskNETContext>(options =>
                options.UseSqlServer(connectionString, builder => builder.MigrationsAssembly(typeof(MagniseTaskNETContext).Assembly.FullName)));

            return services;
        }
    }
}
