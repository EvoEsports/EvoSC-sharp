using System.Reflection;
using EvoSC.Common.Controllers;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Manialinks;

public class ManialinkController : EvoScController<ManialinkInteractionContext>
{
    public Task View(string maniaLink) => Task.CompletedTask;
    public Task View(IOnlinePlayer player, string maniaLink) => Task.CompletedTask;
    public Task View(string maniaLink, dynamic data) => Context.ManialinkManager.SendManialinkAsync(maniaLink, data);
    public Task View(IOnlinePlayer player, string maniaLink, dynamic data) => Task.CompletedTask;
}
