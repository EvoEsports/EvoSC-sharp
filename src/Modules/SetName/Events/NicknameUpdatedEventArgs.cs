using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.SetName.Events;

public class NicknameUpdatedEventArgs : EventArgs
{
    public required IPlayer Player { get; init; }
    public required string OldName { get; init; }
    public required string NewName { get; init; }
}
