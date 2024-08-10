using EvoSC.Common.Events.Arguments;

namespace EvoSC.Modules.Official.ServerManagementModule.Events.Args;

public class PasswordChangedEventArgs : EvoScEventArgs
{
    public string NewPassword { get; init; }
}
