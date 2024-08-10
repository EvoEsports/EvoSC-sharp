using EvoSC.Common.Database.Models.Permissions;
using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("Production")]
[Migration(1722683568)]
public class AddDefaultUserGroups : Migration
{
    public override void Up()
    {
        Insert.IntoTable(DbGroup.TableName)
            .Row(new
            {
                Id = 1,
                Title = "Super Admin",
                Description = "Super admin group with unrestricted permissions.",
                Icon = "",
                Color = "F00",
                Unrestricted = true
            });

        Insert.IntoTable(DbGroup.TableName)
            .Row(new
            {
                Id = 2,
                Title = "Player",
                Description = "Default group for players.",
                Icon = "",
                Color = "FFF",
                Unrestricted = false
            });
    }

    public override void Down()
    {
    }
}
