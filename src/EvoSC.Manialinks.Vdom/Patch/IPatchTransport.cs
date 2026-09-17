using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Manialinks.Vdom.Patch;

/// <summary>
/// Delivers a serialized patch (see <see cref="PatchSerializer"/>) to a set of players without
/// touching their mounted view page. Kept as an interface -- not because M1 needs more than one
/// implementation, but because M0's spike proved exactly one transport mechanism
/// (<see cref="LocalUserPatchTransport"/>) and nothing else; if that assumption ever needs
/// revisiting, swapping the implementation shouldn't touch <c>Views/</c>.
/// </summary>
public interface IPatchTransport
{
    /// <param name="seq">Written to the plain Integer LocalUser seq variable, no serialization needed.</param>
    /// <param name="opsJson">The JSON array from <see cref="PatchSerializer.SerializeOps"/>.</param>
    Task SendAsync(IEnumerable<IPlayer> players, string viewName, int seq, string opsJson);
}
