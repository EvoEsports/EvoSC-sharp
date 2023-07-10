using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Commands.Interfaces;

public interface ICommandInteractionContext : IPlayerInteractionContext
{
    /// <summary>
    /// The command that was executed in this context.
    /// </summary>
    public IChatCommand CommandExecuted { get; init; }
}
