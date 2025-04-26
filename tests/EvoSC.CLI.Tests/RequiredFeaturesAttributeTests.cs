using EvoSC.CLI.Attributes;
using EvoSC.Common.Application;
using Xunit;

namespace EvoSC.CLI.Tests;

public class RequiredFeaturesAttributeTests
{
    [Fact]
    public void Single_Feature_Provided()
    {
        var attr = new RequiredFeaturesAttribute(AppFeature.Auditing);

        Assert.Contains(AppFeature.Auditing, attr.Features);
    }

    [Fact]
    public void Multiple_Features_Is_Combined()
    {
        var attr = new RequiredFeaturesAttribute(AppFeature.Auditing, AppFeature.Config, AppFeature.Database);

        Assert.Equal(new AppFeature[]
        {
            AppFeature.Auditing, 
            AppFeature.Config, 
            AppFeature.Database
        }, attr.Features);
    }

    [Fact]
    public void All_Features_Enum_Adds_All_Features()
    {
        var attr = new RequiredFeaturesAttribute(AppFeature.All);
        
        Assert.Equal(new AppFeature[]
        {
            AppFeature.Config,
            AppFeature.Logging,
            AppFeature.DatabaseMigrations,
            AppFeature.Database,
            AppFeature.Events,
            AppFeature.GbxRemoteClient,
            AppFeature.Modules,
            AppFeature.ControllerManager,
            AppFeature.PlayerManager,
            AppFeature.MapsManager,
            AppFeature.PlayerCache,
            AppFeature.MatchSettings,
            AppFeature.Auditing,
            AppFeature.ServicesManager,
            AppFeature.ChatCommands,
            AppFeature.ActionPipelines,
            AppFeature.Permissions,
            AppFeature.Manialinks,
            AppFeature.Themes,
            AppFeature.Chat,
        }, attr.Features);
    }
}
