using System.Runtime.Serialization;

namespace Claims.Infrastructure.Exceptions;

[Serializable]
internal class DataNotFoundException : Exception
{

    public DataNotFoundException()
    {
    }

    public DataNotFoundException(string? message) : base(message)
    {
    }

    public DataNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}