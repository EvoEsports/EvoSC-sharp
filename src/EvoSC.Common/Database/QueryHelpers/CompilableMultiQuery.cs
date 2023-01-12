using EvoSC.Common.Interfaces.Database;
using SqlKata;
using SqlKata.Compilers;

namespace EvoSC.Common.Database.QueryHelpers;

public class CompilableMultiQuery : ICompilableQuery
{
    private readonly Compiler _compiler;
    private readonly List<Query> _queries = new();

    public Compiler Compiler => _compiler;

    public CompilableMultiQuery(Compiler compiler)
    {
        _compiler = compiler;
    }

    public Query Query()
    {
        var query = new Query();
        _queries.Add(query);
        return query;
    }
    
    public Query Query(string table)
    {
        var query = new Query(table);
        _queries.Add(query);
        return query;
    }

    public CompilableMultiQuery Add(Query query)
    {
        _queries.Add(query);
        return this;
    }

    public (string sql, Dictionary<string, object> values) Compile()
    {
        var compiled = _compiler.Compile(_queries);

        if (compiled == null)
        {
            throw new Exception("Failed to compile query.");
        }

        return (compiled.Sql, compiled.NamedBindings);
    }
}
