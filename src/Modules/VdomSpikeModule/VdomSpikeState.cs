using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;

namespace EvoSC.Modules.Official.VdomSpikeModule;

public interface IVdomSpikeState
{
    int NextSeq();
}

/// <summary>
/// Holds the monotonic patch sequence number across chat command invocations. A singleton so the
/// seq survives regardless of whether controllers are instantiated per-call.
/// </summary>
[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class VdomSpikeState : IVdomSpikeState
{
    private int _seq;

    public int NextSeq() => Interlocked.Increment(ref _seq);
}
