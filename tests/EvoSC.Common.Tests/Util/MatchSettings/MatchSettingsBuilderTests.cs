using System;
using System.Collections.Generic;
using System.Linq;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Common.Util.MatchSettings.Builders;
using EvoSC.Common.Util.MatchSettings.Models;
using EvoSC.Common.Util.MatchSettings.Models.ModeScriptSettingsModels;
using Xunit;

namespace EvoSC.Common.Tests.Util.MatchSettings;

public class MatchSettingsBuilderTests
{
    [Theory]
    [InlineData(DefaultModeScriptName.TimeAttack, "Trackmania/TM_TimeAttack_Online.Script.txt")]
    [InlineData(DefaultModeScriptName.Teams, "Trackmania/TM_Teams_Online.Script.txt")]
    [InlineData(DefaultModeScriptName.Cup, "Trackmania/TM_Cup_Online.Script.txt")]
    [InlineData(DefaultModeScriptName.Knockout, "Trackmania/TM_Knockout_Online.Script.txt")]
    [InlineData(DefaultModeScriptName.Laps, "Trackmania/TM_Laps_Online.Script.txt")]
    [InlineData(DefaultModeScriptName.Rounds, "Trackmania/TM_Rounds_Online.Script.txt")]
    [InlineData(DefaultModeScriptName.TmwtTeams, "Trackmania/TM_TMWTTeams_Online.Script.txt")]
    public void Set_Default_Mode_Script_Name(DefaultModeScriptName name, string expected)
    {
        var builder = new MatchSettingsBuilder();
        builder.WithMode(name);

        var ms = builder.Build();
        
        Assert.Equal(expected, ms.GameInfos.ScriptName);
    }

    [Fact]
    public void Set_Custom_Mode_Script_Name()
    {
        var builder = new MatchSettingsBuilder();

        builder.WithMode("My_Custom_Mode");

        var ms = builder.Build();
        
        Assert.Equal("My_Custom_Mode", ms.GameInfos?.ScriptName);
    }

    [Fact]
    public void GameInfos_Values_Set_Correctly_With_Internal_Builder()
    {
        var msb = new MatchSettingsBuilder().WithGameInfos(gi => gi
            .WithScriptName("My_Script_Name")
            .WithChatTime(123)
            .WithGameMode(5234)
            .AllWarmupDuration(true)
            .ForceShowAllOpponents(true)
            .DisableRespawn(true)
            .WithFinishTimeout(5143)
        );

        var ms = msb.Build();
        
        Assert.NotNull(ms.GameInfos);
        Assert.Equal("My_Script_Name", ms.GameInfos.ScriptName);
        Assert.Equal(123, ms.GameInfos.ChatTime);
        Assert.Equal(5234, ms.GameInfos.GameMode);
        Assert.True(ms.GameInfos.AllWarmupDuration);
        Assert.True(ms.GameInfos.ForceShowAllOpponents);
        Assert.True(ms.GameInfos.DisableRespawn);
        Assert.Equal(5143, ms.GameInfos.FinishTimeout);
    }
    
    [Fact]
    public void GameInfos_Values_Set_Correctly_From_External_Builder()
    {
        var gib = new GameInfosConfigBuilder()
            .WithScriptName("My_Script_Name")
            .WithChatTime(123)
            .WithGameMode(5234)
            .AllWarmupDuration(true)
            .ForceShowAllOpponents(true)
            .DisableRespawn(true)
            .WithFinishTimeout(5143);
        var msb = new MatchSettingsBuilder();

        var ms = msb.WithGameInfos(gib).Build();
        
        Assert.NotNull(ms.GameInfos);
        Assert.Equal("My_Script_Name", ms.GameInfos.ScriptName);
        Assert.Equal(123, ms.GameInfos.ChatTime);
        Assert.Equal(5234, ms.GameInfos.GameMode);
        Assert.True(ms.GameInfos.AllWarmupDuration);
        Assert.True(ms.GameInfos.ForceShowAllOpponents);
        Assert.True(ms.GameInfos.DisableRespawn);
        Assert.Equal(5143, ms.GameInfos.FinishTimeout);
    }

    [Fact]
    public void Set_Filter_With_Internal_Builder()
    {
        var msb = new MatchSettingsBuilder()
            .WithFilter(f => f
                .AsHotseat(true)
                .AsInternet(false)
                .AsLan(false)
                .AsSolo(true)
                .AsRandomMapOrder(true)
                .WithSortIndex(1243)
            );

        var ms = msb.WithMode("Test").Build();
        
        Assert.NotNull(ms.Filter);
        Assert.True(ms.Filter.IsHotseat);
        Assert.False(ms.Filter.IsInternet);
        Assert.False(ms.Filter.IsLan);
        Assert.True(ms.Filter.IsSolo);
        Assert.True(ms.Filter.RandomMapOrder);
        Assert.Equal(1243, ms.Filter.SortIndex);
    }
    
    [Fact]
    public void Set_Filter_With_External_Builder()
    {
        var filter = new FilterConfigBuilder()
            .AsHotseat(true)
            .AsInternet(false)
            .AsLan(false)
            .AsSolo(true)
            .AsRandomMapOrder(true)
            .WithSortIndex(1243);

        var ms = new MatchSettingsBuilder()
            .WithFilter(filter)
            .WithMode("Test")
            .Build();
        
        Assert.NotNull(ms.Filter);
        Assert.True(ms.Filter.IsHotseat);
        Assert.False(ms.Filter.IsInternet);
        Assert.False(ms.Filter.IsLan);
        Assert.True(ms.Filter.IsSolo);
        Assert.True(ms.Filter.RandomMapOrder);
        Assert.Equal(1243, ms.Filter.SortIndex);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(0, 0)]
    [InlineData(3465, 3465)]
    [InlineData(-1, 0)]
    public void Set_Start_Index(int index, int expected)
    {
        var ms = new MatchSettingsBuilder()
            .WithMode("Test")
            .WithStartIndex(index)
            .Build();
        
        Assert.Equal(expected, ms.StartIndex);
    }

    [Fact]
    public void Map_Added_By_Map_Object()
    {
        var ms = new MatchSettingsBuilder()
            .WithMode("Test")
            .AddMap(new Map{ FilePath = "MyMap.Map.Gbx", Uid = "my-uid"})
            .Build();

        Assert.NotNull(ms.Maps);
        
        var map = ms.Maps.FirstOrDefault();
        
        Assert.NotNull(map);
        Assert.Equal("MyMap.Map.Gbx", map.FilePath);
        Assert.Equal("my-uid", map.Uid);
    }
    
    [Fact]
    public void Add_Map_By_File_Name_Only()
    {
        var ms = new MatchSettingsBuilder()
            .WithMode("Test")
            .AddMap("MyMap.Map.Gbx")
            .Build();

        Assert.NotNull(ms.Maps);
        
        var map = ms.Maps.FirstOrDefault();
        
        Assert.NotNull(map);
        Assert.Equal("MyMap.Map.Gbx", map.FilePath);
    }
    
    [Fact]
    public void Add_Map_By_File_Name_And_Uid_String()
    {
        var ms = new MatchSettingsBuilder()
            .WithMode("Test")
            .AddMap("MyMap.Map.Gbx", "my-uid")
            .Build();

        Assert.NotNull(ms.Maps);
        
        var map = ms.Maps.FirstOrDefault();
        
        Assert.NotNull(map);
        Assert.Equal("MyMap.Map.Gbx", map.FilePath);
        Assert.Equal("my-uid", map.Uid);
    }

    [Fact]
    public void Add_Multiple_Maps()
    {
        var maps = new IMap[]
        {
            new Map {FilePath = "one.Map.Gbx", Uid = "uid1"}, 
            new Map {FilePath = "two.Map.Gbx", Uid = "uid2"},
            new Map {FilePath = "three.Map.Gbx", Uid = "uid3"},
        };

        var ms = new MatchSettingsBuilder()
            .WithMode("Test")
            .AddMaps(maps)
            .Build();
        
        Assert.NotNull(ms.Maps);

        var first = ms.Maps.FirstOrDefault();
        Assert.NotNull(first);
        Assert.Equal("one.Map.Gbx", first.FilePath);
        Assert.Equal("uid1", first.Uid);
        
        var second = ms.Maps.Skip(1).FirstOrDefault();
        Assert.NotNull(second);
        Assert.Equal("two.Map.Gbx", second.FilePath);
        Assert.Equal("uid2", second.Uid);
        
        var third = ms.Maps.Skip(2).FirstOrDefault();
        Assert.NotNull(third);
        Assert.Equal("three.Map.Gbx", third.FilePath);
        Assert.Equal("uid3", third.Uid);
    }

    [Fact]
    public void Replace_Maps_With_WithMaps()
    {
        var maps = new IMap[]
        {
            new Map {FilePath = "one.Map.Gbx", Uid = "uid1"}, 
            new Map {FilePath = "two.Map.Gbx", Uid = "uid2"},
            new Map {FilePath = "three.Map.Gbx", Uid = "uid3"},
        };

        var ms = new MatchSettingsBuilder()
            .WithMode("Test")
            .AddMap("to-be-replaced1")
            .AddMap("to-be-replaced2")
            .AddMap("to-be-replaced3")
            .WithMaps(maps)
            .Build();
        
        Assert.NotNull(ms.Maps);

        var first = ms.Maps.FirstOrDefault();
        Assert.NotNull(first);
        Assert.Equal("one.Map.Gbx", first.FilePath);
        Assert.Equal("uid1", first.Uid);
        
        var second = ms.Maps.Skip(1).FirstOrDefault();
        Assert.NotNull(second);
        Assert.Equal("two.Map.Gbx", second.FilePath);
        Assert.Equal("uid2", second.Uid);
        
        var third = ms.Maps.Skip(2).FirstOrDefault();
        Assert.NotNull(third);
        Assert.Equal("three.Map.Gbx", third.FilePath);
        Assert.Equal("uid3", third.Uid);
    }

    [Fact]
    public void Script_Settings_From_CLR_Object()
    {
        var ms = new MatchSettingsBuilder()
            .WithMode(DefaultModeScriptName.TimeAttack)
            .WithModeSettings<TimeAttackModeScriptSettings>(ta => ta.TimeLimit = 1337)
            .Build();
        
        Assert.NotNull(ms.ModeScriptSettings?["S_TimeLimit"]);
        Assert.Equal(1337, ms.ModeScriptSettings["S_TimeLimit"].Value);
    }
    
    [Fact]
    public void Script_Settings_From_CLR_Object_With_Wrong_Mode()
    {

        Assert.Throws<InvalidOperationException>(() =>
        {
            new MatchSettingsBuilder()
                .WithMode(DefaultModeScriptName.Cup)
                .WithModeSettings<TimeAttackModeScriptSettings>(ta => ta.TimeLimit = 1337)
                .Build();
        });
    }
    
    [Fact]
    public void Set_Custom_Script_Settings()
    {
        var ms = new MatchSettingsBuilder()
            .WithMode("Test")
            .WithModeSettings(d => d["S_MySetting"] = 1234)
            .Build();
        
        Assert.NotNull(ms.ModeScriptSettings?["S_MySetting"]);
        Assert.Equal(1234, ms.ModeScriptSettings["S_MySetting"].Value);
    }

    [Fact]
    public void Create_Builder_From_External_MatchSettings()
    {
        var externalMs = new MatchSettingsInfo
        {
            GameInfos = new GameInfosConfigBuilder().WithScriptName("My_Mode").Build(),
            Filter = new FilterConfigBuilder().AsRandomMapOrder(true).Build(),
            ModeScriptSettings = new Dictionary<string, ModeScriptSettingInfo>
            {
                {
                    "S_MySetting",
                    new ModeScriptSettingInfo {Value = 1234, Description = "My Desc", Type = typeof(int)}
                }
            },
            Maps = new List<IMap> {new Map {FilePath = "MyMap.Map.Gbx", Uid = "my-map-uid"}},
            StartIndex = 2456
        };

        var ms = new MatchSettingsBuilder(externalMs).Build();
        
        Assert.NotNull(ms.GameInfos);
        Assert.NotNull(ms.Filter);
        Assert.NotNull(ms.ModeScriptSettings);
        Assert.NotNull(ms.Maps);
        
        Assert.Equal(2456, ms.StartIndex);
        
        Assert.Equal("My_Mode", ms.GameInfos.ScriptName);
        Assert.True(ms.Filter.RandomMapOrder);
        
        Assert.NotNull(ms.ModeScriptSettings["S_MySetting"]);
        Assert.Equal(1234, ms.ModeScriptSettings?["S_MySetting"].Value);
        Assert.Equal("My Desc", ms.ModeScriptSettings?["S_MySetting"].Description);
        Assert.Equal(typeof(int), ms.ModeScriptSettings?["S_MySetting"].Type);
        
        Assert.NotNull(ms.Maps.FirstOrDefault());
        Assert.Equal("MyMap.Map.Gbx", ms.Maps?.FirstOrDefault()?.FilePath);
        Assert.Equal("my-map-uid", ms.Maps?.FirstOrDefault()?.Uid);
    }

    [Fact]
    public void Throws_If_Empty_Gamemode()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            new MatchSettingsBuilder().Build();
        });
    }
}
