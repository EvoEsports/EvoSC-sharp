using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;
using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("XPEvo", "Production")]
[Migration(1695294772)]
public class XPEvoValues : Migration
{
    public override void Up()
    {
        
        Execute.Sql("INSERT INTO \"Groups\" (\"Id\", \"Title\", \"Description\", \"Icon\", \"Color\", \"Unrestricted\") VALUES (1, 'Admin', 'Admin', '', '', true)");

        // Insert.IntoTable(""Players"").Row("Players");

        Execute.Sql("""
                    INSERT INTO "Players" ("Id", "LastVisit", "CreatedAt", "UpdatedAt", "AccountId", "NickName", "UbisoftName", "Zone")
                    VALUES (2, NOW(), NOW(), NOW(), 'b792256a-912b-461c-9f8d-662a040a15ce', 'Karlukki', 'Karlukki', '');
                    INSERT INTO "Players" ("Id", "LastVisit", "CreatedAt", "UpdatedAt", "AccountId", "NickName", "UbisoftName", "Zone")
                    VALUES (3, NOW(), NOW(), NOW(), '18c0c4e6-989a-4aef-b619-6313654f8f48', 'Roxiie', 'Roxiie', '');
                    INSERT INTO "Players" ("Id", "LastVisit", "CreatedAt", "UpdatedAt", "AccountId", "NickName", "UbisoftName", "Zone")
                    VALUES (4, NOW(), NOW(), NOW(), '8e8ea58e-72b1-42eb-bd63-8183bf3add50', 'DoogiieMD', 'DoogiieMD', '');
                    INSERT INTO "Players" ("Id", "LastVisit", "CreatedAt", "UpdatedAt", "AccountId", "NickName", "UbisoftName", "Zone")
                    VALUES (5, NOW(), NOW(), NOW(), '7599d4de-2ced-46d0-abf6-91612e1dca30', 'speq.x', 'speq.x', '');
                    INSERT INTO "Players" ("Id", "LastVisit", "CreatedAt", "UpdatedAt", "AccountId", "NickName", "UbisoftName", "Zone")
                    VALUES (6, NOW(), NOW(), NOW(), '085e1d24-7d55-496d-ad7a-1eb1efec09eb', 'Keissla', 'Keissla', '');
                    INSERT INTO "Players" ("Id", "LastVisit", "CreatedAt", "UpdatedAt", "AccountId", "NickName", "UbisoftName", "Zone")
                    VALUES (7, NOW(), NOW(), NOW(), '6a79abb8-0ffc-4d93-a6e7-2087cc2fd27a', 'Evo.Atomic', 'Evo.Atomic', '');
                    INSERT INTO "Players" ("Id", "LastVisit", "CreatedAt", "UpdatedAt", "AccountId", "NickName", "UbisoftName", "Zone")
                    VALUES (8, NOW(), NOW(), NOW(), '06a503ae-f6da-430b-8d75-901e1302dfb5', 'XLRB.', 'XLRB.', '');
                    INSERT INTO "Players" ("Id", "LastVisit", "CreatedAt", "UpdatedAt", "AccountId", "NickName", "UbisoftName", "Zone")
                    VALUES (9, NOW(), NOW(), NOW(), '39a38ee1-e0a0-49a0-93f5-8024cf1b7f9b', 'Evo.Braker', 'Evo.Braker', '');
                    INSERT INTO "Players" ("Id", "LastVisit", "CreatedAt", "UpdatedAt", "AccountId", "NickName", "UbisoftName", "Zone")
                    VALUES (10, NOW(), NOW(), NOW(), '17868d60-b494-4b88-81df-f4ddfdae1cf1', 'Evo.Chris92', 'Evo.Chris92', '');
                    INSERT INTO "Players" ("Id", "LastVisit", "CreatedAt", "UpdatedAt", "AccountId", "NickName", "UbisoftName", "Zone")
                    VALUES (11, NOW(), NOW(), NOW(), '2496fef1-fed2-44e4-9930-189f46496526', 'ItsPhenom', 'ItsPhenom', '');
                    INSERT INTO "Players" ("Id", "LastVisit", "CreatedAt", "UpdatedAt", "AccountId", "NickName", "UbisoftName", "Zone")
                    VALUES (12, NOW(), NOW(), NOW(), '07c8ae36-fdd3-4685-8050-ba44124dc7e7', 'Biscione156', 'Biscione156', '');
                    """);

        Execute.Sql("INSERT INTO \"UserGroups\" (\"UserId\", \"GroupId\", \"Display\")\nVALUES (6, 1, true), (7, 1, true), (8, 1, true), (9, 1, true), (10, 1, true), (11, 1, true)");
    }

    public override void Down()
    {
        
    }
}
