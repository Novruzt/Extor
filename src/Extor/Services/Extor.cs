using Extor.Interfaces;
using System.Linq.Expressions;
using System.Net;

namespace Extor.Services
{
    /// <summary>
    /// Provides functionality to build and throw custom exceptions, implementing the Extor pattern.
    /// </summary>
    public class Extor : IExtor, IExceptionBuilder, IThrow
    {
        private string _name;
        private string _message = "An error occurred";
        private int _statusCode = (int)HttpStatusCode.InternalServerError;
        private Exception _exception = null;
        private readonly IExtorRegistrar _registrar;
        private bool _nameUsed = false;
        private bool _typeUsed = false;
        private bool _isExtor = false;
        private bool _isMessageUsed = false;
        private bool _isStatusCodeUsed = false;

        /// <summary>
        /// Gets the name of the exception being built.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets the message associated with the exception being built.
        /// </summary>
        public string Message { get { return _message; } }

        /// <summary>
        /// Gets the HTTP status code associated with the exception being built.
        /// </summary>
        public int StatusCode { get { return _statusCode; } }

        /// <summary>
        /// Indicates whether the exception is a custom Extor exception or a standard exception.
        /// </summary>
        public bool IsExtor { get => _isExtor; }

        /// <summary>
        /// Indicates whether the <see cref="WithStatusCode"/> is called.
        /// </summary>
        public bool IsStatusCodeUsed { get => _isStatusCodeUsed; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Extor"/> class.
        /// </summary>
        /// <param name="registrar">The registrar for handling custom assemblies.</param>
        public Extor(IExtorRegistrar registrar)
        {
            _registrar = registrar;
        }

        /// <summary>
        /// Begins the process of creating a new exception.
        /// </summary>
        /// <returns>An instance of <see cref="IExceptionBuilder"/> for configuring the exception.</returns>
        public IExceptionBuilder Create()
        {
            _name = "Exception";
            _message = "An error occured";
            _statusCode = (int)HttpStatusCode.InternalServerError;
            _exception = null;
            _nameUsed =false;
            _typeUsed =false;
            _isExtor = true;
            _isMessageUsed = false;
            _isStatusCodeUsed = false;

            return this;
        }

        /// <summary>
        /// Sets the name of the exception.
        /// </summary>
        /// <param name="name">The name of the exception to be built.</param>
        /// <returns>An instance of <see cref="IExceptionBuilder"/> for further configuration.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="WithType(Exception)"/> has already been called.</exception>
        public IExceptionBuilder WithName(string name)
        {
            if (_typeUsed)
                throw new InvalidOperationException("Cannot use WithName() after WithType().");

            _name = name;
            _nameUsed = true;

            return this;
        }



        /// <summary>
        /// Sets the message of the exception.
        /// </summary>
        /// <param name="message">The message to be associated with the exception.</param>
        /// <returns>An instance of <see cref="IExceptionBuilder"/> for further configuration.</returns>
        public IExceptionBuilder WithMessage(string message)
        {
            _message = message;
            _isMessageUsed=true;
            return this;
        }


        /// <summary>
        /// Sets the HTTP status code for the exception.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to be associated with the exception.</param>
        /// <returns>An instance of <see cref="IExceptionBuilder"/> for further configuration.</returns>
        public IExceptionBuilder WithStatusCode(int statusCode)
        {
            _statusCode = statusCode;
            _isStatusCodeUsed = true;
            return this;
        }

        /// <summary>
        /// Sets the HTTP status code for the exception using <see cref="HttpStatusCode"/>.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to be associated with the exception.</param>
        /// <returns>An instance of <see cref="IExceptionBuilder"/> for further configuration.</returns>
        public IExceptionBuilder WithStatusCode(HttpStatusCode statusCode)
        {
            _statusCode = (int)statusCode;
            _isStatusCodeUsed=true;
            return this;
        }

        /// <summary>
        /// Sets the exception type for the exception being built.
        /// </summary>
        /// <param name="exception">The exception instance representing the type of exception.</param>
        /// <returns>An instance of <see cref="IExceptionBuilder"/> for further configuration.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="WithName(string)"/> has already been called.</exception>
        public IExceptionBuilder WithType(Exception exception)
        {
            if (_nameUsed)
                throw new InvalidOperationException("Cannot use WithType() after WithName().");

            if(!string.IsNullOrEmpty(exception.Message) && !_isMessageUsed)
                 _message = exception.Message;

            _exception = exception;
            _typeUsed = true;

            return this;
        }

        /// <summary>
        /// Builds the exception based on the specified configuration.
        /// </summary>
        /// <returns>An instance of <see cref="IThrow"/> to throw the built exception.</returns>
        public IThrow Build()
        {

            if (_exception == null)
            {
                if (!string.IsNullOrEmpty(_name))
                {
                    _exception = CreateByName(_name) ?? new Exception($"{_name}: {_message}");
                }
                else
                {
                    _exception = new Exception(_message);
                }
            }
            else
            {
                _exception = (Exception)Activator.CreateInstance(_exception.GetType(), _message);
            }

            return this;
        }

        /// <summary>
        /// Throws the built exception.
        /// </summary>
        /// <exception cref="Exception">The exception that was built using the Extor pattern.</exception>
        public void Throw()
        {
            throw _exception;
        }

        /// <summary>
        /// Throws the built exception if the provided predicate function returns true.
        /// </summary>
        /// <param name="predicate">A function that returns a boolean indicating whether the exception should be thrown.</param>
        /// <exception cref="Exception">Thrown if the predicate function returns true and an exception has been set.</exception>
        public void ThrowIf(bool predicate)
        {
            if (predicate)
            {
                Throw();
            }
        }

        private Exception CreateByName(string name)
        {
            var exceptionType = GetByName(name);

            if (exceptionType != null)
            {
                _isExtor = false;
                return (Exception)Activator.CreateInstance(exceptionType, _message);
            }

            return null;
        }

        private Type GetByName(string name)
        {
            // Try finding the type in custom assemblies first
            var type = FindTypeInCustomAssemblies(name);

            // If not found, try in system assemblies
            return type ?? FindTypeInSystemAssemblies(name);
        }

        private Type FindTypeInCustomAssemblies(string name)
        {
            foreach (var assembly in _registrar.GetCustomAssemblies())
            {
                var exceptionType = assembly.GetTypes()
                    .FirstOrDefault(t => t.Name.Equals(name, StringComparison.Ordinal) ||
                                         t.FullName.Equals(name, StringComparison.Ordinal));

                if (exceptionType != null && typeof(Exception).IsAssignableFrom(exceptionType))
                {
                    if(!_isMessageUsed)
                       _message = $"Exception of type {exceptionType.Name} was thrown";
                    return exceptionType;
                }
            }
            return null;
        }

        private Type FindTypeInSystemAssemblies(string name)
        {
            return Type.GetType($"System.{name}, System.Private.CoreLib")
                ?? Type.GetType($"System.{name}, mscorlib");
        }
    }
}
