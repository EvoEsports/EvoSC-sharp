using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.GameModeUiModule.Config;

[Settings]
public interface IGameModeUiModuleSettings
{
    /*
     * Settings for Race_Chrono
     */
    [Option(DefaultValue = true), Description("The visibility of the RaceChrono module.")]
    public bool ChronoVisible { get; set; }

    [Option(DefaultValue = 0.0), Description("The x position of the RaceChrono module.")]
    public double ChronoX { get; set; }

    [Option(DefaultValue = -80.0), Description("The y position of the RaceChrono module.")]
    public double ChronoY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the RaceChrono module.")]
    public double ChronoScale { get; set; }

    /*
     * Settings for Race_RespawnHelper
     */
    [Option(DefaultValue = true), Description("The visibility of the RespawnHelper module.")]
    public bool RespawnHelperVisible { get; set; }

    [Option(DefaultValue = 155.0), Description("The x position of the RaceRespawnHelper module.")]
    public double RespawnHelperX { get; set; }

    [Option(DefaultValue = 15.0), Description("The y position of the RaceRespawnHelper module.")]
    public double RespawnHelperY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the RaceRespawnHelper module.")]
    public double RespawnHelperScale { get; set; }

    /*
     * Settings for Race_Checkpoint
     */
    [Option(DefaultValue = true), Description("The visibility of the Checkpoint module.")]
    public bool CheckpointVisible { get; set; }

    [Option(DefaultValue = -10.0), Description("The x position of the Checkpoint module.")]
    public double CheckpointX { get; set; }

    [Option(DefaultValue = 45.0), Description("The y position of the Checkpoint module.")]
    public double CheckpointY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the Checkpoint module.")]
    public double CheckpointScale { get; set; }

    /*
     * Settings for Race_LapsCounter
     */
    [Option(DefaultValue = true), Description("The visibility of the LapsCounter module.")]
    public bool LapsCounterVisible { get; set; }

    [Option(DefaultValue = -155.7), Description("The x position of the LapsCounter module.")]
    public double LapsCounterX { get; set; }

    [Option(DefaultValue = 86.0), Description("The y position of the LapsCounter module.")]
    public double LapsCounterY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the LapsCounter module.")]
    public double LapsCounterScale { get; set; }

    /*
     * Settings for Race_TimeGap
     */
    [Option(DefaultValue = true), Description("The visibility of the TimeGap module.")]
    public bool TimeGapVisible { get; set; }

    [Option(DefaultValue = 44.0), Description("The x position of the TimeGap module.")]
    public double TimeGapX { get; set; }

    [Option(DefaultValue = -52.0), Description("The y position of the TimeGap module.")]
    public double TimeGapY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the TimeGap module.")]
    public double TimeGapScale { get; set; }

    /*
     * Settings for Race_ScoresTable
     */
    [Option(DefaultValue = true), Description("The visibility of the ScoresTable module.")]
    public bool ScoresTableVisible { get; set; }

    [Option(DefaultValue = 0.0), Description("The x position of the ScoresTable module.")]
    public double ScoresTableX { get; set; }

    [Option(DefaultValue = 0.0), Description("The y position of the ScoresTable module.")]
    public double ScoresTableY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the ScoresTable module.")]
    public double ScoresTableScale { get; set; }
    
    /*
     * Settings for Race_SmallScoresTable
     */
    // TODO: change to true after XPEvo
    [Option(DefaultValue = false), Description("The visibility of the SmallScoresTable module.")]
    public bool SmallScoresTableVisible { get; set; }

    [Option(DefaultValue = 0.0), Description("The x position of the ScoresTable module.")]
    public double SmallScoresTableX { get; set; }

    [Option(DefaultValue = 0.0), Description("The y position of the ScoresTable module.")]
    public double SmallScoresTableY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the ScoresTable module.")]
    public double SmallScoresTableScale { get; set; }

    /*
     * Settings for Race_DisplayMessage
     */
    [Option(DefaultValue = true), Description("The visibility of the DisplayMessage module.")]
    public bool DisplayMessageVisible { get; set; }

    [Option(DefaultValue = -158.0), Description("The x position of the DisplayMessage module.")]
    public double DisplayMessageX { get; set; }

    [Option(DefaultValue = 81.0), Description("The y position of the DisplayMessage module.")]
    public double DisplayMessageY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the DisplayMessage module.")]
    public double DisplayMessageScale { get; set; }

    /*
     * Settings for Race_Countdown
     */
    [Option(DefaultValue = true), Description("The visibility of the Countdown module.")]
    public bool CountdownVisible { get; set; }

    [Option(DefaultValue = 155.0), Description("The x position of the Countdown module.")]
    public double CountdownX { get; set; }

    [Option(DefaultValue = 4.0), Description("The y position of the Countdown module.")]
    public double CountdownY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the Countdown module.")]
    public double CountdownScale { get; set; }

    /*
     * Settings for Race_SpectatorBase_Name
     */
    [Option(DefaultValue = true), Description("The visibility of the SpectatorBaseName module.")]
    public bool SpectatorBaseNameVisible { get; set; }

    [Option(DefaultValue = 0.0), Description("The x position of the SpectatorBaseName module.")]
    public double SpectatorBaseNameX { get; set; }

    [Option(DefaultValue = -60.0), Description("The y position of the SpectatorBaseName module.")]
    public double SpectatorBaseNameY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the SpectatorBaseName module.")]
    public double SpectatorBaseNameScale { get; set; }

    /*
     * Settings for Race_SpectatorBase_Commands
     */
    [Option(DefaultValue = true), Description("The visibility of the SpectatorBaseCommands module.")]
    public bool SpectatorBaseCommandsVisible { get; set; }

    [Option(DefaultValue = 151.0), Description("The x position of the SpectatorBaseCommands module.")]
    public double SpectatorBaseCommandsX { get; set; }

    [Option(DefaultValue = -87.0), Description("The y position of the SpectatorBaseCommands module.")]
    public double SpectatorBaseCommandsY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the SpectatorBaseCommands module.")]
    public double SpectatorBaseCommandsScale { get; set; }

    /*
     * Settings for Race_Record
     */
    [Option(DefaultValue = true), Description("The visibility of the Record module.")]
    public bool RecordVisible { get; set; }

    [Option(DefaultValue = -160.0), Description("The x position of the Record module.")]
    public double RecordX { get; set; }

    [Option(DefaultValue = 30.0), Description("The y position of the Record module.")]
    public double RecordY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the Record module.")]
    public double RecordScale { get; set; }

    /*
     * Settings for Race_BigMessage
     */
    [Option(DefaultValue = true), Description("The visibility of the BigMessage module.")]
    public bool BigMessageVisible { get; set; }

    [Option(DefaultValue = 0.0), Description("The x position of the BigMessage module.")]
    public double BigMessageX { get; set; }

    [Option(DefaultValue = 60.0), Description("The y position of the BigMessage module.")]
    public double BigMessageY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the BigMessage module.")]
    public double BigMessageScale { get; set; }

    /*
     * Settings for Race_BlockHelper
     */
    [Option(DefaultValue = true), Description("The visibility of the BlockHelper module.")]
    public bool BlockHelperVisible { get; set; }

    [Option(DefaultValue = 0.0), Description("The x position of the BlockHelper module.")]
    public double BlockHelperX { get; set; }

    [Option(DefaultValue = 50.0), Description("The y position of the BlockHelper module.")]
    public double BlockHelperY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the BlockHelper module.")]
    public double BlockHelperScale { get; set; }

    /*
     * Settings for Race_WarmUp
     */
    [Option(DefaultValue = true), Description("The visibility of the WarmUp module.")]
    public bool WarmUpVisible { get; set; }

    [Option(DefaultValue = 160.0), Description("The x position of the WarmUp module.")]
    public double WarmUpX { get; set; }

    [Option(DefaultValue = -10.0), Description("The y position of the WarmUp module.")]
    public double WarmUpY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the WarmUp module.")]
    public double WarmUpScale { get; set; }

    /*
     * Settings for Race_BestRaceViewer
     */
    [Option(DefaultValue = true), Description("The visibility of the BestRaceViewer module.")]
    public bool BestRaceViewerVisible { get; set; }

    [Option(DefaultValue = 135.0), Description("The x position of the BestRaceViewer module.")]
    public double BestRaceViewerX { get; set; }

    [Option(DefaultValue = -10.0), Description("The y position of the BestRaceViewer module.")]
    public double BestRaceViewerY { get; set; }

    [Option(DefaultValue = 1.0), Description("The scale of the BestRaceViewer module.")]
    public double BestRaceViewerScale { get; set; }
}
