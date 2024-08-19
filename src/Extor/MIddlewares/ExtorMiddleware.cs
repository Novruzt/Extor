using Extor.Interfaces;
using Extor.Services;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using static Extor.Services.ExceptionMapping;

namespace Extor.Middlewares
{
    /// <summary>
    /// Middleware for handling exceptions in the application.
    /// </summary>
    public class ExtorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExtor _extor;
        private const int DefaultStatusCode = (int)HttpStatusCode.InternalServerError;
        private const string DefaultErrorMessage = "An unexpected error occurred.";

        /// <summary>
        /// Initializes a new instance of the ExtorMiddleware class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="extor">The Extor instance.</param>
        public ExtorMiddleware(RequestDelegate next, IExtor extor)
        {
            _next = next;
            _extor = extor;
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var type = ex.GetType();

                var mapping = ExceptionMappingRegistry.Mappings.FirstOrDefault(m => m.ExceptionType == ex.GetType());
                if (mapping != null)
                    await HandleMappedAsync(context, ex, mapping);

                else if (_extor.IsExtor)
                    await HandleExtorAsync(context, ex);
                else
                    await HandleDefaultAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles exceptions with a registered mapping.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="ex">The exception.</param>
        /// <param name="mapping">The exception mapping.</param>
        private async Task HandleMappedAsync(HttpContext context, Exception ex, ExceptionMapping mapping)
        {
            string message;

            if (mapping.OverrideMessage)
                message = mapping.Message;

            else if(!string.IsNullOrEmpty(ex.Message))
                message = ex.Message;

            else if(mapping.Message != null)
                message= mapping.Message;
            else 
                message =DefaultErrorMessage;

            var exceptionResult = JsonSerializer.Serialize(new { Result = "Failed", Error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = _extor.IsStatusCodeUsed ? _extor.StatusCode : mapping.StatusCode;


            await context.Response.WriteAsync(exceptionResult);
        }

        /// <summary>
        /// Handles exceptions using Extor's dynamic exception handling.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="ex">The exception.</param>
        private async Task HandleExtorAsync(HttpContext context, Exception ex)
        {
            int statusCode = _extor.StatusCode;
            string name = _extor.Name;
            string message = _extor.Message;

            var exceptionResult = JsonSerializer.Serialize(new { Result = "Failed", Error = name, Message = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(exceptionResult);
        }

        /// <summary>
        /// Handles exceptions with default behavior.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="ex">The exception.</param>
        private async Task HandleDefaultAsync(HttpContext context, Exception ex)
        {
            var exceptionResult = JsonSerializer.Serialize(new { Result = "Failed", Error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = DefaultStatusCode;

            await context.Response.WriteAsync(exceptionResult);
        }
    }
}
