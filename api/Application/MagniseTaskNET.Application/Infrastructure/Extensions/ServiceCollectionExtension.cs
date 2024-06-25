using MagniseTaskNET.Application.Interfaces;
using MagniseTaskNET.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MagniseTaskNET.Application.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IApiService, ApiService>();

            return services;
        }
    }
}
