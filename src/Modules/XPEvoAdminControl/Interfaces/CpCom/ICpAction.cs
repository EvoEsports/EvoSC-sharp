namespace EvoSC.Modules.Official.XPEvoAdminControl.Interfaces.CpCom;

public interface ICpAction
{
    public string AccessToken { get; set; }
    public string Action { get; set; }
    public object Data { get; set; }

    public Guid ActionId { get; set; }
}
