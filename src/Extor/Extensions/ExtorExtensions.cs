using Extor.Interfaces;
using Extor.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Extor.Extensions
{
    /// <summary>
    /// Extension methods for configuring Extor services in the dependency injection container.
    /// </summary>
    public static class ExtorExtensions
    {
        /// <summary>
        /// Registers Extor services and assemblies containing custom exceptions.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="exceptionAssemlies">Array of types representing assemblies containing custom exceptions.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddExtor(this IServiceCollection services, params Type[] exceptionAssemlies)
        {
            services.AddSingleton<IExtorRegistrar, ExtorRegistrar>();

            services.AddSingleton<IExtor>(provider =>
            {
                var registrar = provider.GetRequiredService<IExtorRegistrar>();
                var extor = new Extor.Services.Extor(registrar);

                foreach (Type type in exceptionAssemlies)
                    extor.AddAssembly(registrar, type);

                return extor;
            });

            return services;
        }

        /// <summary>
        /// Registers a custom exception assembly with Extor.
        /// </summary>
        /// <param name="extor">The Extor instance.</param>
        /// <param name="registrar">The Extor registrar instance.</param>
        /// <param name="exceptionType">The type representing the assembly containing custom exceptions.</param>
        /// <returns>The updated Extor instance.</returns>
        private static IExtor AddAssembly(this IExtor extor, IExtorRegistrar registrar, Type exceptionType)
        {
            registrar.RegisterCustomAssembly(exceptionType);
            return extor;
        }
    }
}
