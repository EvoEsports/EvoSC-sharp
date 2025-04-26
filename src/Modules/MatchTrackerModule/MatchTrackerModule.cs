using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Stores;

namespace EvoSC.Modules.Official.MatchTrackerModule;

[Module(IsInternal = true)]
public class MatchTrackerModule : EvoScModule
{
    public MatchTrackerModule(IDatabaseMatchTrackerStore dbStore, ITrackerStoreService stores)
    {
        stores.AddStore(dbStore);
    }
}
