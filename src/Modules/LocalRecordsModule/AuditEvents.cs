using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.LocalRecordsModule;

public enum AuditEvents
{
    [Identifier(Name = "LocalRecords:ResetAll")]
    ResetAll,
}
