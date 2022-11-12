namespace EvoSC.Modules.Official.Maps.Interfaces;

public interface IMxMapService
{
    Task FindAndDownloadMap(int mxId, string? shortName);
}
