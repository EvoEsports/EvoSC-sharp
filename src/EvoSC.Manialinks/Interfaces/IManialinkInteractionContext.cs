using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Interfaces;

public interface IManialinkInteractionContext : IPlayerInteractionContext
{
    /// <summary>
    /// Information about the manialink action that occured.
    /// </summary>
    public IManialinkActionContext ManialinkAction { get; init; }

    /// <summary>
    /// The manialink manager service.
    /// </summary>
    public IManialinkManager ManialinkManager { get; init; }
}
