namespace EvoSC.Modules.Official.ContractApiProviderModule.Contracts;

/// <summary>
/// A match result DTO exchanged between modules through <see cref="IScoreboardService"/>.
/// Not annotated itself: it is discovered automatically from the exported interface's
/// methods and emitted into the export assembly alongside the interface, so producers
/// and consumers share the same type identity.
/// </summary>
public sealed record MatchResult(string MatchId, string BlueTeam, string RedTeam, int BlueScore, int RedScore);