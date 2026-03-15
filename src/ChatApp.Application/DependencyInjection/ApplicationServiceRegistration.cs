using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
