using AppGestionGastos.Application.Abstractions.DomainEvents;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AppGestionGastos.Application;

public static class DependencyInjection
{
    private static readonly Type[] HandlerInterfaces =
    [
        typeof(ICommandHandler<,>),
        typeof(IQueryHandler<,>),
        typeof(IDomainEventHandler<>),
        typeof(IPostCommitDomainEventHandler<>)
    ];

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
        services.AddHandlers(assembly);

        return services;
    }

    private static void AddHandlers(this IServiceCollection services, System.Reflection.Assembly assembly)
    {
        var registrations =
            from type in assembly.GetTypes()
            where type is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false }
            from @interface in type.GetInterfaces()
            where @interface.IsGenericType && HandlerInterfaces.Contains(@interface.GetGenericTypeDefinition())
            select (Service: @interface, Implementation: type);

        foreach (var (service, implementation) in registrations)
        {
            services.AddScoped(service, implementation);
        }
    }
}
