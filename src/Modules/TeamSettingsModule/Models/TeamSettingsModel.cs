using System.ComponentModel.DataAnnotations;
using EvoSC.Manialinks.Attributes;
using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.TeamSettingsModule.Models;

[FormEntryModel]
public class TeamSettingsModel
{
    /// <summary>
    /// The name of the team.
    /// </summary>
    [Required(AllowEmptyStrings=false)]
    public string Team1Name { get; set; }

    /// <summary>
    /// The primary color of the team.
    /// </summary>
    [Required, MinLength(3), MaxLength(6)]
    public string Team1PrimaryColor { get; set; } = "0000ff";

    /// <summary>
    /// The secondary color of the team.
    /// </summary>
    [MaxLength(6)]
    public string? Team1SecondaryColor { get; set; }

    /// <summary>
    /// The logo for the team.
    /// </summary>
    // [Url]
    public string? Team1EmblemUrl { get; set; }

    /// <summary>
    /// The name of the team.
    /// </summary>
    [Required(AllowEmptyStrings=false)]
    public string Team2Name { get; set; }

    /// <summary>
    /// The color of the team.
    /// </summary>
    [Required, MinLength(3), MaxLength(6)]
    public string Team2PrimaryColor { get; set; } = "ff0000";

    /// <summary>
    /// The color of the team.
    /// </summary>
    [MaxLength(6)]
    public string? Team2SecondaryColor { get; set; }

    /// <summary>
    /// The logo for the team.
    /// </summary>
    // [Url]
    public string? Team2EmblemUrl { get; set; }
}
