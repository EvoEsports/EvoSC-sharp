namespace EvoSC.Common.Interfaces.Models;

public interface IPlayerSettings
{
    public string DisplayLanguage { get; set; }

    public string? HiddenManialinks { get; set; }

    public List<string> GetHiddenManialinks();

    public void SetHiddenManialinks(List<string> hiddenManialinks);
}
