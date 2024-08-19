using System.Runtime.Serialization;

namespace Extor.Exceptions;
public class DefaultException : Exception
{
    public DefaultException()
    {
    }

    public DefaultException(string? message) : base(message)
    {
    }

    public DefaultException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DefaultException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
