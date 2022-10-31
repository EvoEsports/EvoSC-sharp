using System.Reflection;

namespace EvoSC.Common.Interfaces;

public interface IMigrationManager
{
    /// <summary>
    /// Migrate the database from migrations found in an assembly.
    /// </summary>
    /// <param name="asm"></param>
    public void MigrateFromAssembly(Assembly asm);
}
