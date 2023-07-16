using System.Diagnostics.CodeAnalysis;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MotdModule.Interfaces;

public interface IMotdEntry
{
    public IPlayer Player { get; }
}
