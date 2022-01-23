using System;
using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Infrastructure;

namespace EvoSC.Migrations
{
    [Migration(20210823214300)]
    public class CreateDatabase : Migration 
    {
        public override void Up()
        {
            Create.Table("Players")
                .WithColumn("ID").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("Login").AsString()
                .WithColumn("Nickname").AsString()
                .WithColumn("UbisoftName").AsString()
                .WithColumn("Group").AsInt32()
                .WithColumn("Path").AsString()
                .WithColumn("Banned").AsBoolean()
                .WithColumn("LastVisit").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime);
            
            Create.Table("Maps")
                .WithColumn("ID").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("UID").AsString().Unique()
                .WithColumn("PlayerID").AsInt32()
                .ForeignKey("FK_Maps_PlayerID", "Players", "ID")
                .WithColumn("FilePath").AsString().WithColumnDescription("Relative to UserData/Maps")
                .WithColumn("Enabled").AsBoolean()
                .WithColumn("Name").AsString()
                .WithColumn("ManiaExchangeId").AsInt32()
                .WithColumn("ManiaExchangeVersion").AsDateTime()
                .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("UpdatedAt").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime);
            
            Create.Table("Player_MapFavorites")
                .WithColumn("PlayerID").AsInt32()
                .ForeignKey("FK_MapFavorites_PlayerID", "Players", "ID")
                .WithColumn("MapID").AsInt32()
                .ForeignKey("FK_MapFavorites_MapID", "Maps", "ID");

            Create.Table("Player_PersonalBests")
                .WithColumn("ID").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("PlayerID").AsInt32()
                .ForeignKey("FK_PersonalBests_PlayerID", "Players", "ID")
                .WithColumn("MapID").AsInt32()
                .ForeignKey("FK_PersonalBests_MapID", "Maps", "ID")
                .WithColumn("Score").AsInt32()
                .WithColumn("Checkpoints").AsInt32()
                .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("UpdatedAt").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime);
            
            Create.Table("Player_Statistics")
                .WithColumn("PlayerID").AsInt32().PrimaryKey().NotNullable()
                .ForeignKey("FK_Statistics_PlayerID", "Players", "ID")
                .WithColumn("Visits").AsInt32()
                .WithColumn("PlayTime").AsInt32()
                .WithColumn("Finishes").AsInt32()
                .WithColumn("LocalRecords").AsInt32()
                .WithColumn("Ratings").AsInt32()
                .WithColumn("Wins").AsInt32()
                .WithColumn("Donations").AsInt32()
                .WithColumn("Score").AsInt32()
                .WithColumn("Rank").AsInt32()
                .WithColumn("SpectatorTime").AsInt32()
                .WithColumn("CheckpointsDriven").AsInt32()
                .WithColumn("ConsecutiveDaysPlayed").AsInt32()
                .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("UpdatedAt").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime);
            
            Create.Table("Map_Karma")
                .WithColumn("ID").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("MapID").AsInt32()
                .ForeignKey("FK_Map_Karma_MapID", "Maps", "ID")
                .WithColumn("PlayerID").AsInt32()
                .ForeignKey("FK_Map_Karma_PlayerID", "Players", "ID")
                .WithColumn("Rating").AsInt16()
                .WithColumn("New").AsBoolean();

            Create.Table("Map_Statistics")
                .WithColumn("MapID").AsInt32().PrimaryKey().NotNullable()
                .ForeignKey("FK_Map_Statistics_MapID", "Maps", "ID")
                .WithColumn("NumberOfPlays").AsInt32()
                .WithColumn("Cooldown").AsInt32()
                .WithColumn("LastPlayed").AsDateTime()
                .WithColumn("AmountSkipped").AsInt32();

            Create.Table("Map_Records")
                .WithColumn("ID").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("PlayerID").AsInt32()
                .ForeignKey("FK_LocalRecords_PlayerID", "Players", "ID")
                .WithColumn("MapID").AsInt32()
                .ForeignKey("FK_LocalRecords_MapID", "Maps", "ID")
                .WithColumn("Score").AsInt32()
                .WithColumn("Rank").AsInt32()
                .WithColumn("Checkpoints").AsString()
                .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("UpdatedAt").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime);
            
            Create.PrimaryKey("PK_MapFavorites").OnTable("Player_MapFavorites").WithSchema("Player_MapFavorites")
                .Columns("PlayerID", "MapID");
        }

        public override void Down()
        {
            Delete.Table("Players");
            Delete.Table("Maps");
            Delete.Table("Player_MapFavorites");
            Delete.Table("Player_PersonalBests");
            Delete.Table("Player_Statistics");
            Delete.Table("Map_Karma");
            Delete.Table("Map_Records");
            Delete.Table("Map_Statistics");
        }
    }
}
