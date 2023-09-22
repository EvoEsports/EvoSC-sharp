using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("XPEvo", "Production")]
[Migration(1695388994)]
public class AddSnix : Migration {
    public override void Up()
    {
        Execute.Sql("""
            INSERT INTO "Players" ("Id", "LastVisit", "CreatedAt", "UpdatedAt", "AccountId", "NickName", "UbisoftName", "Zone")
            VALUES (69, NOW(), NOW(), NOW(), 'a467a996-eba5-44bf-9e2b-8543b50f39ae', 'snixtho', 'snixtho', '');
        """);

        Execute.Sql("INSERT INTO \"UserGroups\" (\"UserId\", \"GroupId\", \"Display\")\nVALUES (69, 1, true)");
    }
                    
    public override void Down()
    {
    }
}

                    
