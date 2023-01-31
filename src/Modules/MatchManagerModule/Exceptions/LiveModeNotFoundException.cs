namespace EvoSC.Modules.Official.MatchManagerModule.Exceptions;

public class LiveModeNotFoundException : Exception
{
    public LiveModeNotFoundException(string name) : base($"The mode '{name}' was not found.") { }
}
