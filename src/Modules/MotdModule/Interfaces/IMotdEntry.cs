using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MotdModule.Interfaces;

public interface IMotdEntry
{
    /// <summary>
    /// The player of the MotdEntry.
    /// </summary>
    public IPlayer Player { get; }
}
