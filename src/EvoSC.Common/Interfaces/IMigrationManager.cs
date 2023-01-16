using System.Reflection;

namespace EvoSC.Common.Interfaces;

public interface IMigrationManager
{
    /// <summary>
    /// Migrate the database from migrations found in an assembly.
    /// </summary>
    /// <param name="asm">Assembly that contains migrations to search for.</param>
    /// <param name="dbType">The chosen database type.</param>
    public void MigrateFromAssembly(Assembly asm);
}
