using EvoSC.Manialinks.Vdom.Mount;

namespace EvoSC.Manialinks.Vdom.Vdom;

/// <summary>
/// One leaf as currently mounted: the <see cref="FlatNode"/> last diffed for it, plus the real
/// pool slot it occupies. A view keeps the full list of these between renders (see
/// <c>Views/IManialinkView</c>) and passes it back into the next <see cref="Reconciler.Diff"/>
/// call as <c>previouslyMounted</c>.
/// </summary>
public readonly record struct MountedNode(FlatNode Flat, int Slot);
