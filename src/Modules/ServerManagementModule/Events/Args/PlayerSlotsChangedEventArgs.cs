using EvoSC.Common.Events.Arguments;

namespace EvoSC.Modules.Official.ServerManagementModule.Events.Args;

public class PlayerSlotsChangedEventArgs : EvoScEventArgs
{
    public int NewSlots { get; init; }
}
