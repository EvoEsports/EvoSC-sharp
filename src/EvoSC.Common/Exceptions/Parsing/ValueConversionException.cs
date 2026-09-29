namespace EvoSC.Common.Exceptions.Parsing;

public class ValueConversionException : Exception
{
    public ValueConversionException()
    {
    }

    public ValueConversionException(string message) : base(message)
    {
    }
}