using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Testing.Controllers;

public class CommandInteractionControllerTestBase<TController> : ControllerMock<TController, ICommandInteractionContext>
    where TController : class, IController
{
    /// <summary>
    /// Initialize this controller mock.
    /// </summary>
    /// <param name="actor">The player that triggered the command.</param>
    /// <param name="services">Services for the controller. Can be Mock objects or plain objects.</param>
    protected void InitMock(IOnlinePlayer actor, params object[] services)
    {
        base.InitMock(services);
        this.SetupMock(actor);
    }
}

