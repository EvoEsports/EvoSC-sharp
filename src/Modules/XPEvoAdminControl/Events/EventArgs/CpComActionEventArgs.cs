using EvoSC.Modules.Official.XPEvoAdminControl.Interfaces.CpCom;

namespace EvoSC.Modules.Official.XPEvoAdminControl.Events.EventArgs;

public class CpComActionEventArgs : System.EventArgs
{
    public ICpAction Action { get; init; }
}
