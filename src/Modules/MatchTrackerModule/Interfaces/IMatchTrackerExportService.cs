namespace EvoSC.Modules.Official.MatchTrackerModule.Interfaces;

public interface IMatchTrackerExportService
{
    public Task ExportCsv(string file);
}
