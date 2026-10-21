using System.ComponentModel.DataAnnotations;
using EvoSC.Manialinks.Attributes;

namespace EvoSC.Modules.Official.GroupsModule.Models;

[FormEntryModel]
public class GroupEditModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "The title can't be empty.")]
    [MaxLength(40, ErrorMessage = "The title can be at most 40 characters.")]
    public string Title { get; set; } = "";

    [MaxLength(200, ErrorMessage = "The description can be at most 200 characters.")]
    public string? Description { get; set; }

    [MaxLength(2, ErrorMessage = "The icon must be a single character.")]
    public string? Icon { get; set; }

    [RegularExpression("^([0-9a-fA-F]{3}|[0-9a-fA-F]{6})?$",
        ErrorMessage = "Please provide a valid 3 or 6 digit color code, or leave empty.")]
    public string? Color { get; set; }

    public bool Unrestricted { get; set; }

    public bool ShowOnScoreboard { get; set; }
}
