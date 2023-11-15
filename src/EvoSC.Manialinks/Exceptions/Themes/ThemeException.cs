using System.Runtime.Serialization;

namespace EvoSC.Manialinks.Exceptions.Themes;

public class ThemeException : Exception
{
    public ThemeException()
    {
    }

    protected ThemeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ThemeException(string? message) : base(message)
    {
    }

    public ThemeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
