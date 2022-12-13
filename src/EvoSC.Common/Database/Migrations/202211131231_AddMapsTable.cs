using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Migration(1668338922)]
public class AddMapsTable : Migration
{
    public override void Up()
    {
        Create.Table("Maps")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Uid").AsString().Unique()
            .WithColumn("AuthorId").AsInt32()
            .WithColumn("FilePath").AsString()
            .WithColumn("Enabled").AsBoolean()
            .WithColumn("Name").AsString()
            .WithColumn("ExternalId").AsString().Unique().Nullable()
            .WithColumn("ExternalVersion").AsDateTime().Nullable()
            .WithColumn("ExternalMapProvider").AsString().Nullable()
            .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("UpdatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);

        Create.ForeignKey("FK_Maps_Players")
            .FromTable("Maps").ForeignColumn("AuthorId")
            .ToTable("Players").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.Table("Maps");
    }
}
