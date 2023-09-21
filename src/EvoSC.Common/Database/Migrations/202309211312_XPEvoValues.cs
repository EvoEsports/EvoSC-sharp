using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Database.Models.Player;
using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("XPEvo")]
[Migration(1695294772)]
public class AddAdminGroup : Migration
{
    public override void Up()
    {
        var groups = new Dictionary<string, DbGroup>();
        groups.Add("1", new DbGroup {
            Id = 1,
            Title = "Admin",
            Description = "Admin",
            Icon = "",
            Color = "",
            Unrestricted = true
        });
        groups.Add("2", new DbGroup
        {
            Id = 2,
            Title = "Observer",
            Description = "Observer",
            Icon = null,
            Color = null,
            Unrestricted = true
        });
        Insert.IntoTable("Groups").Row(groups);

        var players = new Dictionary<string, DbPlayer>();
        players.Add("1", new DbPlayer
        {
            Id = 1,
            LastVisit = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AccountId = "b792256a-912b-461c-9f8d-662a040a15ce",
            NickName = "Karlukki",
            UbisoftName = "Karlukki",
            Zone = ""
        });
        players.Add("2", new DbPlayer
        {
            Id = 2,
            LastVisit = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AccountId = "18c0c4e6-989a-4aef-b619-6313654f8f48",
            NickName = "Roxiie",
            UbisoftName = "Roxiie",
            Zone = ""
        });
        players.Add("3", new DbPlayer
        {
            Id = 3,
            LastVisit = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AccountId = "8e8ea58e-72b1-42eb-bd63-8183bf3add50",
            NickName = "DoogiieMD",
            UbisoftName = "DoogiieMD",
            Zone = ""
        });
        players.Add("4", new DbPlayer
        {
            Id = 4,
            LastVisit = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AccountId = "7599d4de-2ced-46d0-abf6-91612e1dca30",
            NickName = "speq.x",
            UbisoftName = "speq.x",
            Zone = ""
        });
        players.Add("5", new DbPlayer
        {
            Id = 5,
            LastVisit = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AccountId = "085e1d24-7d55-496d-ad7a-1eb1efec09eb",
            NickName = "Keissla",
            UbisoftName = "Keissla",
            Zone = ""
        });
        players.Add("6", new DbPlayer
        {
            Id = 6,
            LastVisit = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AccountId = "6a79abb8-0ffc-4d93-a6e7-2087cc2fd27a",
            NickName = "Evo.Atomic",
            UbisoftName = "Evo.Atomic",
            Zone = ""
        });
        players.Add("7", new DbPlayer
        {
            Id = 7,
            LastVisit = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AccountId = "06a503ae-f6da-430b-8d75-901e1302dfb5",
            NickName = "XLRB.",
            UbisoftName = "XLRB.",
            Zone = ""
        });
        players.Add("8", new DbPlayer
        {
            Id = 8,
            LastVisit = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AccountId = "06a503ae-f6da-430b-8d75-901e1302dfb5",
            NickName = "Evo.Braker",
            UbisoftName = "Evo.Braker",
            Zone = ""
        });
        players.Add("9", new DbPlayer
        {
            Id = 9,
            LastVisit = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AccountId = "17868d60-b494-4b88-81df-f4ddfdae1cf1",
            NickName = "Evo.Chris92",
            UbisoftName = "Evo.Chris92",
            Zone = ""
        });
        players.Add("10", new DbPlayer
        {
            Id = 10,
            LastVisit = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AccountId = "2496fef1-fed2-44e4-9930-189f46496526",
            NickName = "ItsPhenom",
            UbisoftName = "ItsPhenom",
            Zone = ""
        });
        players.Add("11", new DbPlayer
        {
            Id = 11,
            LastVisit = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AccountId = "07c8ae36-fdd3-4685-8050-ba44124dc7e7",
            NickName = "Biscione156",
            UbisoftName = "Biscione156",
            Zone = ""
        });
        players.Add("12",
            new DbPlayer
            {
                Id = 13,
                LastVisit = DateTime.Now,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                AccountId = "f46b8742-e352-405d-a9e5-0d208f667023",
                NickName = "ImportTM",
                UbisoftName = "ImportTM",
                Zone = ""
            });
        players.Add("13", new DbPlayer
        {
            Id = 14,
            LastVisit = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AccountId = "4c702731-5630-4108-b10c-d672f6e321ed",
            NickName = "LuckersTurbo",
            UbisoftName = "LuckersTurbo",
            Zone = ""
        });

        var userGroups = new Dictionary<string, DbUserGroup>();
        
        userGroups.Add("1", new DbUserGroup { UserId = 6, GroupId = 1, Display = true });
        userGroups.Add("2", new DbUserGroup { UserId = 7, GroupId = 1, Display = true });
        userGroups.Add("3", new DbUserGroup { UserId = 8, GroupId = 1, Display = true });
        userGroups.Add("4", new DbUserGroup { UserId = 9, GroupId = 1, Display = true });
        userGroups.Add("5", new DbUserGroup { UserId = 10, GroupId = 1, Display = true });
        userGroups.Add("6", new DbUserGroup { UserId = 11, GroupId = 1, Display = true });
        userGroups.Add("7", new DbUserGroup { UserId = 12, GroupId = 2, Display = true });
        userGroups.Add("8", new DbUserGroup { UserId = 13, GroupId = 2, Display = true });
        userGroups.Add("9", new DbUserGroup { UserId = 14, GroupId = 2, Display = true });
        

        Insert.IntoTable("UserGroups").Row(userGroups);
    }

    public override void Down()
    {
        
    }
}
