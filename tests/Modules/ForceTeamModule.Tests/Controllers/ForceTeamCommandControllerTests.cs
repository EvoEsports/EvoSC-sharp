using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.ForceTeamModule.Controllers;
using EvoSC.Modules.Official.ForceTeamModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace ForceTeamModule.Tests.Controllers;

public class ForceTeamCommandControllerTests : CommandInteractionControllerTestBase<ForceTeamCommandController>
{
    private readonly Mock<IForceTeamService> _forceTeamService = new();
    private readonly Mock<IOnlinePlayer> _player = new();
    
    public ForceTeamCommandControllerTests()
    {
        InitMock(_player.Object, _forceTeamService);
    }

    [Fact]
    public async Task Window_Is_Shown()
    {
        await Controller.ForceTeamAsync();
        
        _forceTeamService.Verify(m => m.ShowWindowAsync(_player.Object));
    }

    [Fact]
    public async Task Auto_Team_Balance_Triggered()
    {
        await Controller.AutoBalanceAsync();
        
        _forceTeamService.Verify(m => m.BalanceTeamsAsync());
    }
}
