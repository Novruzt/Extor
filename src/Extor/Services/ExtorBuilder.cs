using Extor.Interfaces;
using Microsoft.AspNetCore.Builder;
using System.Net;
using static Extor.Services.ExceptionMapping;

namespace Extor.Services;

/// <summary>
/// Provides methods to configure exception handling for the Extor middleware in an ASP.NET Core application.
/// </summary>
public class ExtorBuilder:IExtorBuilder
{
    private readonly IApplicationBuilder _app;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtorBuilder"/> class.
    /// </summary>
    /// <param name="app">The application builder used to configure the middleware.</param>
    public ExtorBuilder(IApplicationBuilder app)
    {
        _app = app;
    }

    /// <summary>
    /// Registers a custom exception type with a specific HTTP status code and optional message.
    /// </summary>
    /// <param name="exceptionType">The type of the exception to handle.</param>
    /// <param name="globalStatusCode">The HTTP status code to associate with the exception. Ignored if <see cref=IExceptionBuilder.WithStatusCode"/> is used/>
    /// <param name="globalMessage">An optional message to use when the exception is thrown.</param>
    /// <param name="overrideMessage">Specifies whether the provided message should override the exception's original message.</param>
    /// <returns>An instance of <see cref="IExtorBuilder"/> to allow for method chaining.</returns>
    public IExtorBuilder Handle(Type exceptionType, int globalStatusCode, string globalMessage = null, bool overrideMessage = false)
    {
        ExceptionMappingRegistry.AddMapping(exceptionType, globalStatusCode, globalMessage, overrideMessage);
        return this;
    }

    /// <summary>
    /// Registers a custom exception type with a specific <see cref="HttpStatusCode"/> and optional message.
    /// </summary>
    /// <param name="exceptionType">The type of the exception to handle.</param>
    /// <param name="globalStatusCode">The <see cref="HttpStatusCode"/> to associate with the exception. Ignored if <see cref=IExceptionBuilder.WithStatusCode"/> is used/></param>
    /// <param name="globalMessage">An optional message to use when the exception is thrown.</param>
    /// <param name="overrideMessage">Specifies whether the provided message should override the exception's original message.</param>
    /// <returns>An instance of <see cref="IExtorBuilder"/> to allow for method chaining.</returns>
    public IExtorBuilder Handle(Type exceptionType, HttpStatusCode globalStatusCode, string globalMessage = null, bool overrideMessage = false)
    {
        ExceptionMappingRegistry.AddMapping(exceptionType, globalStatusCode, globalMessage, overrideMessage);
        return this;
    }
}
