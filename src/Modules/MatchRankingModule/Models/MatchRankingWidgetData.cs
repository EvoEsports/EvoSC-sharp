namespace EvoSC.Modules.Official.MatchRankingModule.Models;

public record MatchRankingWidgetData
{
    public required int Position { get; init; }
    public required int Points { get; init; }
    public required string Name { get; init; }
}
