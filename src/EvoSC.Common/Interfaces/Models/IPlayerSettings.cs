namespace EvoSC.Common.Interfaces.Models;

public interface IPlayerSettings
{
    public string DisplayLanguage { get; set; }

    public List<string> HiddenManialinks { get; set; }
}
