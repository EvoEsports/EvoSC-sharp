using System.ComponentModel.DataAnnotations;
using EvoSC.Manialinks.Attributes;

namespace EvoSC.Modules.Official.SetName.Models;

[EntryModel]
public class SetNameEntryModel
{
    [MinLength(1)]
    public string Nickname { get; set; }
}
