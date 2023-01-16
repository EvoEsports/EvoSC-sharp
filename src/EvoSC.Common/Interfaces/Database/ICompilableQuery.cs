using SqlKata.Compilers;

namespace EvoSC.Common.Interfaces.Database;

public interface ICompilableQuery
{
    public Compiler Compiler { get; }

    public (string sql, Dictionary<string, object> values) Compile();
}
