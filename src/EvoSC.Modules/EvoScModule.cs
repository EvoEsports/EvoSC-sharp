using EvoSC.Common.Interfaces;

namespace EvoSC.Modules;

public abstract class EvoScModule : IEvoScModule
{
    public virtual Type[] Controllers { get; } = Array.Empty<Type>();
}
