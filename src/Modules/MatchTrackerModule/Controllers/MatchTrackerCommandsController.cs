using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchTrackerModule.Controllers;

[Controller]
public class MatchTrackerCommandsController(IMatchTrackerExportService exportService) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("matchtrackerexport", "Export all match tracker data to a csv file.")]
    [CommandAlias("/mtexport", true)]
    public Task ExportCsvAsync(string file) => exportService.ExportCsv(file);
}
