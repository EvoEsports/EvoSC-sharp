using System.ComponentModel;
using Config.Net;
using EvoSC.Common.Config.Models;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.LocalRecordsModule.Config;

[Settings]
public interface ILocalRecordsSettings
{
    [Option(DefaultValue = 10), Description("Max of rows to show in the local records widget.")]
    public int MaxWidgetRows { get; }
    
    [Option(DefaultValue = 3), Description("Always show top N players in the widget.")]
    public int WidgetShowTop { get; }
    
    [Option(DefaultValue = 100), Description("Maximum number of local records to keep track of per map.")]
    public int MaxRecordsPerMap { get; }
}

