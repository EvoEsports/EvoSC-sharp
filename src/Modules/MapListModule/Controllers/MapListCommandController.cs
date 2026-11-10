using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Modules.Official.MapListModule.Permissions;

namespace EvoSC.Modules.Official.MapListModule.Controllers;

[Controller]
public class MapListCommandController(IMapListService mapList, IMapService mapService, IChatService chat) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("maplist", "Show a list of available maps.")]
    [CommandAlias("/maps")]
    [CommandAlias("/tracklist")]
    [CommandAlias("/tracks")]
    public Task MapListAsync() => mapList.ShowMapListAsync(Context.Player);

    [ChatCommand("addtag", "Add a tag to the current map.", MapListPermissions.ManageTags)]
    public async Task AddTagAsync(string tag)
    {
        var map = await mapService.GetCurrentMapAsync();

        if (map != null && await mapList.AddTagAsync(map.Uid, tag))
        {
            await chat.SuccessMessageAsync($"Added the tag '{tag}' to '{map.Name}'.", Context.Player);
            return;
        }

        await chat.ErrorMessageAsync("Could not add the tag. Tags must be 1-32 characters.", Context.Player);
    }

    [ChatCommand("removetag", "Remove a tag from the current map.", MapListPermissions.ManageTags)]
    public async Task RemoveTagAsync(string tag)
    {
        var map = await mapService.GetCurrentMapAsync();

        if (map != null && await mapList.RemoveTagAsync(map.Uid, tag))
        {
            await chat.SuccessMessageAsync($"Removed the tag '{tag}' from '{map.Name}'.", Context.Player);
            return;
        }

        await chat.ErrorMessageAsync($"The current map does not have the tag '{tag}'.", Context.Player);
    }
}
