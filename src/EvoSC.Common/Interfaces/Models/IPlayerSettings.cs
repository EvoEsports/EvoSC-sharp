namespace EvoSC.Common.Interfaces.Models;

public interface IPlayerSettings
{
    public string DisplayLanguage { get; set; }

    public string? HiddenManialinks { get; set; }

    public IEnumerable<string> GetHiddenManialinks();

    public void SetHiddenManialinks(List<string> hiddenManialinks);
}
