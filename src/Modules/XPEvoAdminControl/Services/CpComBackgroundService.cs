using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Modules.Official.XPEvoAdminControl.Interfaces;
using EvoSC.Common.Interfaces;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Services;
using EvoSC.Modules.Official.MatchTrackerModule.Models;
using EvoSC.Modules.Official.XPEvoAdminControl.Models;
using EvoSC.Modules.Official.XPEvoAdminControl.Models.CpCom;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.XPEvoAdminControl.Services;

[Service]
public class CpComBackgroundService : IBackgroundService
{
    private readonly ICpComService _cpComService;
    private readonly IGeardownService _geardown;
    private readonly IGeardownSetupStateService _geardownState;
    private readonly IProblemService _problems;
    private readonly IServerClient _server;
    private readonly ILogger<CpComBackgroundService> _logger;

    private Task _updateTask;
    private readonly CancellationTokenSource _cancelSource = new();

    public CpComBackgroundService(ICpComService cpComService, IGeardownService geardown,
        IGeardownSetupStateService geardownState, IProblemService problems, IServerClient server,
        ILogger<CpComBackgroundService> logger)
    {
        _cpComService = cpComService;
        _geardown = geardown;
        _geardownState = geardownState;
        _problems = problems;
        _server = server;
        _logger = logger;
    }

    public Task StartAsync()
    {
        _updateTask = Task.Run(UpdateStatus);
        
        return Task.CompletedTask;
    }

    private async Task UpdateStatus()
    {
        while (!_cancelSource.IsCancellationRequested)
        {
            try
            {
                var matchStatus = _geardown.GetMatchStatus();

                var status = matchStatus switch
                {
                    MatchStatus.Running => MatchServerStatus.Running,
                    MatchStatus.Ended => MatchServerStatus.Finished,
                    MatchStatus.Started => MatchServerStatus.Running,
                    MatchStatus.UnPaused => MatchServerStatus.Running,
                    _ => MatchServerStatus.Unknown
                };

                var pauseStatus = await _server.Remote.GetModeScriptResponseAsync("Maniaplanet.Pause.GetStatus");

                if (_geardownState.WaitingForMatchStart)
                {
                    status = MatchServerStatus.Ready;
                }

                if (!_geardownState.SetupFinished)
                {
                    status = MatchServerStatus.Waiting;
                }

                if (_problems.GetPlayersWithAProblem().Any())
                {
                    status = MatchServerStatus.Problem;
                }

                await _cpComService.UpdateAsync(new CpAction { Action = "Status", Data = status });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update status.");
            }

            await Task.Delay(TimeSpan.FromSeconds(3));
        }
    }

    public async Task StopAsync()
    {
        _cancelSource.Cancel();
        await _updateTask;
    }
}
    
