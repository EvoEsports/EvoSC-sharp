using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.InternalFixtureModule;

/// <summary>
/// A module that declares itself internal, the way the modules that ship with EvoSC do. It exists
/// to cover loading a module that the application registered by id, from a fixed directory.
/// </summary>
[Module(IsInternal = true)]
public class InternalFixtureModule : EvoScModule
{
}
