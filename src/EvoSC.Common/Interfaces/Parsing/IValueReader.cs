namespace EvoSC.Common.Interfaces.Parsing;

public interface IValueReader
{
    public IEnumerable<Type> AllowedTypes { get; }
    public Task<object> Read(Type type, string input);
}
