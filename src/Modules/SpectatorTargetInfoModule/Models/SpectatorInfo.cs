namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Models;

public record SpectatorInfo(
    bool IsSpectator,
    bool IsTemporarySpectator,
    bool IsPureSpectator,
    bool IsAutoTarget,
    int TargetPlayerId
);
