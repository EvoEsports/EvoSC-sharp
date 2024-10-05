using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Nsgr.ContactAdminModule.Interfaces;

namespace EvoSC.Modules.Nsgr.ContactAdminModule;

[Module]
public class ContactAdminModule(IContactAdminService service) : EvoScModule, IToggleable
{
    public Task EnableAsync() => service.ShowWidgetAsync();
    public Task DisableAsync() => service.HideWidgetAsync();
}
