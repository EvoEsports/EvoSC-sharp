using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using GbxRemoteNet.Interfaces;
using Moq;

namespace EvoSC.Testing.Controllers;

public class CommandInteractionControllerTestBase<TController> : ControllerMock<TController, ICommandInteractionContext>
    where TController : class, IController
{
    protected readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote, Mock<IChatService> Chat) Server =
        Mocking.NewServerClientMock();
    
    /// <summary>
    /// Initialize this controller mock.
    /// </summary>
    /// <param name="actor">The player that triggered the command.</param>
    /// <param name="services">Services for the controller. Can be Mock objects or plain objects.</param>
    protected void InitMock(IOnlinePlayer actor, params object[] services)
    {
        base.InitMock(services);
        this.SetupMock(Server.Client, actor);
    }
}

