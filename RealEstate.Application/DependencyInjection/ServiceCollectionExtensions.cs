using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Mappings;
using RealEstate.Application.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper: registra todos los perfiles en el ensamblado donde está MappingProfile
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Servicios de aplicación
        services.AddScoped<IPropertyService, PropertyService>();

        return services;
    }
}
