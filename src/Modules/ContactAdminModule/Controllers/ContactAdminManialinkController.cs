using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Modules.Nsgr.ContactAdminModule.Interfaces;

namespace EvoSC.Modules.Nsgr.ContactAdminModule.Controllers;

[Controller]
public class ContactAdminManialinkController(IContactAdminService service) : ManialinkController
{
    public Task ContactAdminButtonAsync() => service.ContactAdminAsync();
}