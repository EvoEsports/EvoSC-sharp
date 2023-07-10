using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using Moq;

namespace EvoSC.Testing.Controllers;

public class CommandInteractionControllerTestBase<TController> : ControllerMock<TController, ICommandInteractionContext>
    where TController : class, IController
{
    protected void InitMock(IOnlinePlayer actor, params Mock[] services)
    {
        base.InitMock(services);
        this.SetupMock(actor);
    }
}

