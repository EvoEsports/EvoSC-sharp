using EvoSC.Common.Interfaces.Database;
using SqlKata;
using SqlKata.Compilers;

namespace EvoSC.Common.Database.Repository;

public class CompilableQuery : Query, ICompilableQuery
{
    private readonly Compiler _compiler;
    
    public Compiler Compiler => _compiler;
    
    public CompilableQuery(Compiler compiler)
    {
        _compiler = compiler;
    }
    
    public CompilableQuery(string table, Compiler compiler) : base(table)
    {
        _compiler = compiler;
    }
    
    public CompilableQuery(string table, Compiler compiler, string comment) : base(table, comment)
    {
        _compiler = compiler;
    }
    
    public (string sql, Dictionary<string, object> values) Compile()
    {
        var compiled = _compiler.Compile(this);

        if (compiled == null)
        {
            throw new Exception("Failed to compile query.");
        }

        return (compiled.Sql, compiled.NamedBindings);
    }
}
