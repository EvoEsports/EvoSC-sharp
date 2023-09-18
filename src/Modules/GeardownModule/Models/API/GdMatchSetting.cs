namespace EvoSC.Modules.Evo.GeardownModule.Models.API;

public class GdMatchSetting
{
    public int? id { get; set; }
    public string? key { get; set; } //S_WhateverOption
    public string? value { get; set; }
    public int? format_id { get; set; }
}