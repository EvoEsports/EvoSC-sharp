using System.Reflection;

namespace EvoSC.Common.Interfaces;

public interface IMigrationManager
{
    public void MigrateFromAssembly(Assembly asm);
}
