using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using NSubstitute;

namespace EvoSC.Testing.Controllers;

public class ManialinkControllerTestBase<TController> : ControllerMock<TController, IManialinkInteractionContext>
    where TController : class, IController
{
    private readonly IManialinkManager _mlManager = Substitute.For<IManialinkManager>();

    /// <summary>
    /// The manialink manager mock used for this mock.
    /// </summary>
    public IManialinkManager ManialinkManager => _mlManager;
    
    /// <summary>
    /// Initialize this controller mock.
    /// </summary>
    /// <param name="actor">The player that triggered the command.</param>
    /// <param name="actionContext">The manialink action context for this mock.</param>
    /// <param name="services">Services for the controller. Can be Mock objects or plain objects.</param>
    protected void InitMock(IOnlinePlayer actor, IManialinkActionContext actionContext, params object[] services)
    {
        base.InitMock(services);
        this.SetupMock(actor, actionContext, _mlManager);
    }
}
