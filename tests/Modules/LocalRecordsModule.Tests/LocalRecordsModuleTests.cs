using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;
using Moq;

namespace LocalRecordsModule.Tests;

public class LocalRecordsModuleTests
{
    [Fact]
    public async Task Widget_Is_Shown_To_All_On_Enable()
    {
        var manialinks = new Mock<IManialinkManager>();
        var localRecords = new Mock<ILocalRecordsService>();
        var module = new EvoSC.Modules.Official.LocalRecordsModule.LocalRecordsModule(manialinks.Object, localRecords.Object);

        await module.EnableAsync();
        
        localRecords.Verify(m => m.ShowWidgetToAllAsync());
    }
    
    [Fact]
    public async Task Widget_Is_Hidden_On_Disable()
    {
        var manialinks = new Mock<IManialinkManager>();
        var localRecords = new Mock<ILocalRecordsService>();
        var module = new EvoSC.Modules.Official.LocalRecordsModule.LocalRecordsModule(manialinks.Object, localRecords.Object);

        await module.DisableAsync();

        manialinks.Verify(m => m.HideManialinkAsync("LocalRecordsModule.LocalRecordsWidget"));
    }
}
