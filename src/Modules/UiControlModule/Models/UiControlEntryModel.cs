using System.ComponentModel.DataAnnotations;
using EvoSC.Manialinks.Attributes;

namespace EvoSC.Modules.Official.UiControlModule.Models;

[FormEntryModel]
public class UiControlEntryModel
{
    [Required] public string HiddenManialinks { get; set; }
}
