namespace EvoSC.Modules.Evo.GeardownModule.Interfaces.Services;

public interface IGeardownSetupStateService
{
    public bool IsInitialSetup { get; }
    public string MatchSettingsName { get; }
    public bool SetupFinished { get; }
    public bool WaitingForMatchStart { get; }
    
    public void SetInitialSetup(string matchSettingsName);
    public void SetSetupFinished();
    public void SetMatchStarted();
}
