using System.Linq.Expressions;
using System.Net;

namespace Extor.Interfaces
{
    /// <summary>
    /// Interface representing the Extor service for dynamic exception handling.
    /// </summary>
    public interface IExtor
    {
        /// <summary>
        /// Creates a new exception builder.
        /// </summary>
        /// <returns>An exception builder instance.</returns>
        IExceptionBuilder Create();

        /// <summary>
        /// Gets the name of the exception.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the message of the exception.
        /// Default value is "An error occurred".
        /// </summary>
        string Message { get; }

        //// <summary>
        /// Gets the HTTP status code associated with the exception.
        /// Default value is 400 (Bad Request).
        /// </summary>

        int StatusCode { get; }

        /// <summary>
        /// Gets a value indicating whether the exception was created using Extor.
        /// </summary>
        bool IsExtor { get; }

        /// Indicates whether the <see cref="IExceptionBuilder.WithStatusCode"/> is called.
        bool IsStatusCodeUsed { get; }
    }

    /// <summary>
    /// Interface for building exceptions dynamically.
    /// </summary>
    public interface IExceptionBuilder
    {
        /// <summary>
        /// Sets the name of the exception.
        /// Throws <see cref="InvalidOperationException"/> if <see cref="WithType"/> has already been called.
        /// If an exception class is found by the given name, it will be thrown, otherwise it will be Exception type.
        /// <param name="name">The name of the exception class.</param>
        /// <returns>The exception builder instance.</returns>
        IExceptionBuilder WithName(string name);

        /// <summary>
        /// Sets the message of the exception.
        /// Default value is "An error occurred".
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <returns>The exception builder instance.</returns>
        IExceptionBuilder WithMessage(string message);

        /// <summary>
        /// Sets the status code of the exception.
        /// Default value is 400 (Bad Request).
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns>The exception builder instance.</returns>
        IExceptionBuilder WithStatusCode(int statusCode);

        /// <summary>
        /// Sets the status code of the exception using an <see cref="HttpStatusCode"/> enum.
        /// Default value is 500 (InternalServerSerror).
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns>The exception builder instance.</returns>
        IExceptionBuilder WithStatusCode(HttpStatusCode statusCode);

        /// <summary>
        /// Sets the type of the exception.
        /// Throws <see cref="InvalidOperationException"/> if <see cref="WithName"/> has already been called.
        /// Uses the exception's message if <see cref="WithMessage"/> is not called.
        /// </summary>
        /// <param name="exception">The exception type.</param>
        /// <returns>The exception builder instance.</returns>
        IExceptionBuilder WithType(Exception exception);

        /// <summary>
        /// Builds the exception.
        /// </summary>
        /// <returns>An <see cref="IThrow"/> instance representing the built exception.</returns>
        IThrow Build();
    }

    /// <summary>
    /// Interface for throwing exceptions.
    /// </summary>
    public interface IThrow
    {
        /// <summary>
        /// Throws the built exception.
        /// </summary>
        void Throw();

        /// <summary>
        /// Throws the exception if the provided predicate returns true.
        /// </summary>
        /// <param name="predicate">A function that returns a boolean indicating whether the built exception should be thrown.</param>
        void ThrowIf(bool predicate);
    }
}
