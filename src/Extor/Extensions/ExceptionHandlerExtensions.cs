using Extor.Interfaces;
using Extor.Middlewares;
using Extor.Services;
using Microsoft.AspNetCore.Builder;

namespace Extor.Extensions
{
    /// <summary>
    /// Extension methods for configuring Extor middleware in the application.
    /// </summary>
    public static class ExceptionHandlerExtensions
    {
        /// <summary>
        /// Configures the application to use the Extor middleware.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>An IExtorBuilder instance for further configuration.</returns>
        public static IExtorBuilder UseExtor(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExtorMiddleware>();
            return new ExtorBuilder(app);
        }
    }
}
