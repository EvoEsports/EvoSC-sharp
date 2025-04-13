using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MatchSettingsEditorModule.Interfaces;

namespace EvoSC.Modules.Official.MatchSettingsEditorModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchSettingsEditorService(IManialinkManager manialinks, IMatchSettingsService matchSettingsService) : IMatchSettingsEditorService
{
    public Task ShowEditorAsync()
    {
        throw new NotImplementedException();
    }

    private async IEnumerable<IMatchSettings> GetAllMatchSettingsAsync()
    {
        
    }
}
