using EvoSC.Common.Interfaces;

namespace EvoSC.Modules;

public interface IEvoScModule
{
    public Type[] Controllers { get; }
}
