﻿using FluentMigrator;

namespace EvoSC.Modules.Official.PlayerRecords.Database.Migrations;

[Migration(1672217440)]
public class AddPlayerRecordsTable : Migration
{
    public override void Up()
    {
        Create.Table("PlayerRecords")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("PlayerId").AsInt32().Indexed()
            .WithColumn("MapId").AsInt32().Indexed()
            .WithColumn("Score").AsInt32()
            .WithColumn("Checkpoints").AsString().Nullable()
            .WithColumn("RecordType").AsString()
            .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("UpdatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
    }

    public override void Down()
    {
        Delete.Table("PlayerRecords");
    }
}
