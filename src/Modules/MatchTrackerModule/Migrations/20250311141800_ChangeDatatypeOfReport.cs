using EvoSC.Modules.Official.MatchTrackerModule.Models.Database;
using FluentMigrator;

namespace EvoSC.Modules.Official.MatchTrackerModule.Migrations;

[Migration(1741699715)]
public class ChangeDatatypeOfReport : Migration {
    
    public const string TableName = "MatchRecords";
    public const string ReportColumn = "Report";
    
    public override void Up()
    {
        IfDatabase("mysql").Alter.Table(TableName).AlterColumn(ReportColumn).AsCustom("LONGTEXT");
    }
    
    public override void Down()
    {
        IfDatabase("mysql").Alter.Table(TableName).AlterColumn(ReportColumn).AsCustom("TEXT");
    } 
}
