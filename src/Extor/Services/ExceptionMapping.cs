using System.Net;

namespace Extor.Services;

/// <summary>
/// Represents a mapping of an exception type to a status code and message.
/// </summary>
public class ExceptionMapping
{
    /// <summary>
    /// Gets  the type of the exception.
    /// </summary>
    public Type ExceptionType { get; private set; }

    /// <summary>
    /// Gets the HTTP status code to be returned when the exception occurs.
    /// </summary>
    public int StatusCode { get; private set; }

    /// <summary>
    /// Gets the message to be returned when the exception occurs.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Gets a value indicating whether to override the exception's message with the specified message.
    /// </summary>
    public bool OverrideMessage { get; private set; }


    /// <summary>
    /// Provides methods to register and manage exception mappings.
    /// </summary>
    public static class ExceptionMappingRegistry
    {
        /// <summary>
        /// A list of all registered exception mappings.
        /// </summary>

        public static readonly List<ExceptionMapping> Mappings = new List<ExceptionMapping>();

        /// <summary>
        /// Adds a mapping for a specific exception type with an integer status code.
        /// </summary>
        /// <param name="exceptionType">The type of the exception.</param>
        /// <param name="statusCode">The HTTP status code to be returned.</param>
        /// <param name="message">The message to be returned, if provided; otherwise, a default message is used.</param>
        /// <param name="overrideMessage">Indicates whether to override the exception's message with the specified message.</param>
        public static void AddMapping(Type exceptionType, int statusCode, string message = null, bool overrideMessage = false)
        {
            Mappings.Add(new ExceptionMapping
            {
                ExceptionType = exceptionType,
                StatusCode = statusCode,
                Message = message ?? "An error occurred",
                OverrideMessage = overrideMessage
            });
        }

        /// <summary>
        /// Adds a mapping for a specific exception type with an HttpStatusCode.
        /// </summary>
        /// <param name="exceptionType">The type of the exception.</param>
        /// <param name="statusCode">The HTTP status code to be returned.</param>
        /// <param name="message">The message to be returned, if provided; otherwise, a default message is used.</param>
        /// <param name="overrideMessage">Indicates whether to override the exception's message with the specified message.</param>
        public static void AddMapping(Type exceptionType, HttpStatusCode statusCode, string message = null, bool overrideMessage = false)
        {
            Mappings.Add(new ExceptionMapping
            {
                ExceptionType = exceptionType,
                StatusCode = (int)statusCode,
                Message = message ?? "An error occurred",
                OverrideMessage = overrideMessage
            });
        }
    }
}
