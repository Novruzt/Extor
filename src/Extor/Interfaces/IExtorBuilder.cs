using System.Net;


namespace Extor.Interfaces;

/// <summary>
/// Interface for configuring Extor exception handling.
/// </summary>
public interface IExtorBuilder
{
    /// <summary>
    /// Handles a specific exception type with a custom configuration.
    /// </summary>
    /// <param name="exceptionType">The exception type.</param>
    /// <param name="globalStatusCode">The HTTP status code to return. Ignored if <see cref=IExceptionBuilder.WithStatusCode"/> is used/></param>
    /// <param name="message">The message to return. Default is null.</param>
    /// <param name="overrideMessage">Whether to override the exception's message. Default is false</param>
    IExtorBuilder Handle(Type exceptionType, int globalStatusCode, string message = null, bool overrideMessage = false);

    /// <summary>
    /// Handles a specific exception type with a custom configuration.
    /// </summary>
    /// <param name="exceptionType">The exception type.</param>
    /// <param name="globalStatusCode">The HTTP status code to return. Ignored if <see cref=IExceptionBuilder.WithStatusCode"/> is used/></param>
    /// <param name="message">The message to return. Default is null.</param>
    /// <param name="overrideMessage">Whether to override the exception's message. Default is false.</param>
    IExtorBuilder Handle(Type exceptionType, HttpStatusCode globalStatusCode, string message = null, bool overrideMessage = false);
}
