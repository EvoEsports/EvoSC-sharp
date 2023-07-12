using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using Moq;

namespace EvoSC.Testing.Controllers;

public class ManialinkControllerTestBase<TController> : ControllerMock<TController, IManialinkInteractionContext>
    where TController : class, IController
{
    private Mock<IManialinkManager> _mlManager = new();

    public Mock<IManialinkManager> ManialinkManager => _mlManager;
    
    protected void InitMock(IOnlinePlayer actor, IManialinkActionContext actionContext, params object[] services)
    {
        base.InitMock(services);
        this.SetupMock(actor, actionContext, _mlManager.Object);
    }
}
