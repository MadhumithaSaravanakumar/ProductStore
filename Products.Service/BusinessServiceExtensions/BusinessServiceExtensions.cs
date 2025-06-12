using Microsoft.Extensions.DependencyInjection;
using Products.Repository.Interfaces;
using Products.Repository;
using System.Reflection;

namespace Products.Service.BusinessServiceExtensions
{
    public static class BusinessServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Register repository and service dependencies
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
