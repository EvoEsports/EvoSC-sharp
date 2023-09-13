using EvoSC.Modules.Official.XPEvoAdminControl.Interfaces.CpCom;

namespace EvoSC.Modules.Official.XPEvoAdminControl.Models.CpCom;

public class CpAction : ICpAction {
    public string AccessToken { get; set; }
    public string Action { get; set; }
    public object Data { get; set; }
    public Guid ActionId { get; set; }
}
