using System.ComponentModel.DataAnnotations;
using EvoSC.Manialinks.Attributes;

namespace EvoSC.Modules.Official.SetName.Models;

[FormEntryModel]
public class SetNameEntryModel
{
    /// <summary>
    /// The new desired nickname of the player.
    /// </summary>
    [MinLength(1)]
    public string Nickname { get; set; }
}
