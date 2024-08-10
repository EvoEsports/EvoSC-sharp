using System.Linq;
using System.Threading.Tasks;
using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Database.Repository.Permissions;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Permissions.Models;
using EvoSC.Testing;
using EvoSC.Testing.Database;
using LinqToDB;
using Xunit;

namespace EvoSC.Common.Tests.Database;

public class PermissionRepositoryTests
{
    private static (PermissionRepository, IDbConnectionFactory) CreateNewRepository()
    {
        var factory = TestDbSetup.CreateDb(typeof(AddPlayersTable).Assembly);
        return (new PermissionRepository(factory, TestLoggerSetup.CreateLogger<PermissionRepository>()), factory);
    }

    [Fact]
    public async Task Permission_Added()
    {
        var (repo, dbFactory) = CreateNewRepository();

        await repo.AddPermissionAsync(new Permission {Name = "MyPermission", Description = "My permission description."});

        var perm = await dbFactory.GetConnection().GetTable<DbPermission>().FirstOrDefaultAsync();
        
        Assert.NotNull(perm);
        Assert.Equal("MyPermission", perm.Name);
        Assert.Equal("My permission description.", perm.Description);
    }

    [Fact]
    public async Task Permission_Updated()
    {
        var (repo, dbFactory) = CreateNewRepository();

        await repo.AddPermissionAsync(new Permission {Name = "MyPermission", Description = "My permission description."});
        var oldPerm = await dbFactory.GetConnection().GetTable<DbPermission>().FirstAsync();

        oldPerm.Name = "MyUpdatedPermission";
        await repo.UpdatePermissionAsync(oldPerm);
        
        var newPerm = await dbFactory.GetConnection().GetTable<DbPermission>().FirstAsync();
        
        Assert.Equal("MyUpdatedPermission", newPerm.Name);
    }

    [Fact]
    public async Task Permission_Retrieved_By_Name()
    {
        var (repo, dbFactory) = CreateNewRepository();

        await repo.AddPermissionAsync(new Permission {Name = "MyPermission", Description = "My permission description."});

        var perm = await repo.GetPermissionAsync("MyPermission");
        
        Assert.NotNull(perm);
        Assert.Equal("MyPermission", perm.Name);
        Assert.Equal("My permission description.", perm.Description);
    }

    [Fact]
    public async Task Get_All_Permissions_Of_A_Player()
    {
        var (repo, dbFactory) = CreateNewRepository();

        var player = await DbTestHelper.AddTestPlayer(dbFactory);
        var group = await repo.AddGroupAsync(new Group {Title = "My Group", Description = "My group description."});

        var permsToAdd = new IPermission[]
        {
            new Permission {Name = "MyPermission1", Description = "My permission 1 description."},
            new Permission {Name = "MyPermission2", Description = "My permission 2 description."},
            new Permission {Name = "MyPermission3", Description = "My permission 3 description."}
        };

        for (var i = 0; i < permsToAdd.Length; i++)
        {
            permsToAdd[i] = await repo.AddPermissionAsync(permsToAdd[i]);
            await repo.AddPermissionToGroupAsync(group.Id, permsToAdd[i].Id);
        }

        await repo.AddPlayerToGroupAsync(player.Id, group.Id);

        var permissions = await repo.GetPlayerPermissionsAsync(player.Id);

        var perm1 = permissions.FirstOrDefault(p => p.Id == permsToAdd.First().Id);
        Assert.NotNull(perm1);
        Assert.Equal("MyPermission1", perm1.Name);
        Assert.Equal("My permission 1 description.", perm1.Description);
        
        var perm2 = permissions.FirstOrDefault(p => p.Id == permsToAdd.Skip(1).First().Id);
        Assert.NotNull(perm2);
        Assert.Equal("MyPermission2", perm2.Name);
        Assert.Equal("My permission 2 description.", perm2.Description);
        
        var perm3 = permissions.FirstOrDefault(p => p.Id == permsToAdd.Skip(2).First().Id);
        Assert.NotNull(perm3);
        Assert.Equal("MyPermission3", perm3.Name);
        Assert.Equal("My permission 3 description.", perm3.Description);
    }

    [Fact]
    public async Task Permission_Removed_And_Cleaned_From_Database()
    {
        var (repo, dbFactory) = CreateNewRepository();
        
        var group = await repo.AddGroupAsync(new Group {Title = "My Group", Description = "My group description."});
        var permission = await repo.AddPermissionAsync(new Permission
        {
            Name = "MyPermission", Description = "My permission description."
        });
        await repo.AddPermissionToGroupAsync(group.Id, permission.Id);

        await repo.RemovePermissionAsync(permission);

        var existingPerm = await dbFactory.GetConnection().GetTable<DbPermission>().FirstOrDefaultAsync();
        var existingRef = await dbFactory.GetConnection().GetTable<DbGroupPermission>().FirstOrDefaultAsync();
        
        Assert.Null(existingPerm);
        Assert.Null(existingRef);
    }

    [Fact]
    public async Task Get_All_Player_Groups()
    {
        var (repo, dbFactory) = CreateNewRepository();
        
        var player = await DbTestHelper.AddTestPlayer(dbFactory);
        var group1 = await repo.AddGroupAsync(new Group {Title = "MyGroup 1", Description = "MyGroup 1 description."});
        var group2 = await repo.AddGroupAsync(new Group {Title = "MyGroup 2", Description = "MyGroup 2 description."});
        var group3 = await repo.AddGroupAsync(new Group {Title = "MyGroup 3", Description = "MyGroup 3 description."});

        await repo.AddPlayerToGroupAsync(player.Id, group1.Id);
        await repo.AddPlayerToGroupAsync(player.Id, group2.Id);
        await repo.AddPlayerToGroupAsync(player.Id, group3.Id);

        var playerGroups = await repo.GetGroupsAsync(player.Id);

        var playerGroup1 = playerGroups.FirstOrDefault(g => g.Id == group1.Id);
        Assert.NotNull(playerGroup1);
        Assert.Equal("MyGroup 1", playerGroup1.Title);
        Assert.Equal("MyGroup 1 description.", playerGroup1.Description);
        
        var playerGroup2 = playerGroups.FirstOrDefault(g => g.Id == group2.Id);
        Assert.NotNull(playerGroup2);
        Assert.Equal("MyGroup 2", playerGroup2.Title);
        Assert.Equal("MyGroup 2 description.", playerGroup2.Description);
        
        var playerGroup3 = playerGroups.FirstOrDefault(g => g.Id == group3.Id);
        Assert.NotNull(playerGroup3);
        Assert.Equal("MyGroup 3", playerGroup3.Title);
        Assert.Equal("MyGroup 3 description.", playerGroup3.Description);
    }

    [Fact]
    public async Task Group_Added()
    {
        var (repo, dbFactory) = CreateNewRepository();

        await repo.AddGroupAsync(new Group
        {
            Title = "MyGroup",
            Description = "MyGroup description.",
            Icon = "A",
            Color = "aaa",
            Unrestricted = true
        });

        var group = await dbFactory.GetConnection().GetTable<DbGroup>().Skip(2).FirstOrDefaultAsync();
        
        Assert.NotNull(group);
        Assert.Equal("MyGroup", group.Title);
        Assert.Equal("MyGroup description.", group.Description);
        Assert.Equal("A", group.Icon);
        Assert.Equal("aaa", group.Color);
        Assert.True(group.Unrestricted);
        Assert.NotEqual(0, group.Id);
    }

    [Fact]
    public async Task Group_Details_Updated()
    {
        var (repo, dbFactory) = CreateNewRepository();

        var group = await repo.AddGroupAsync(new Group
        {
            Title = "MyGroup",
            Description = "MyGroup description.",
            Icon = "A",
            Color = "aaa",
            Unrestricted = false
        });

        group.Title = "MyUpdatedGroup";
        group.Description = "MyUpdatedGroup description.";
        group.Icon = "b";
        group.Color = "bbb";
        group.Unrestricted = true;

        await repo.UpdateGroupAsync(group);
        
        var updatedGroup = await dbFactory.GetConnection().GetTable<DbGroup>().Skip(2).FirstOrDefaultAsync();
        
        Assert.NotNull(updatedGroup);
        Assert.Equal("MyUpdatedGroup", updatedGroup.Title);
        Assert.Equal("MyUpdatedGroup description.", updatedGroup.Description);
        Assert.Equal("b", updatedGroup.Icon);
        Assert.Equal("bbb", updatedGroup.Color);
        Assert.True(updatedGroup.Unrestricted);
    }

    [Fact]
    public async Task Group_Removed_And_Cleaned_From_Database()
    {
        var (repo, dbFactory) = CreateNewRepository();

        var player = DbTestHelper.AddTestPlayer(dbFactory);
        var group = await repo.AddGroupAsync(new Group
        {
            Title = "MyGroup",
            Description = "MyGroup description.",
            Icon = "A",
            Color = "aaa",
            Unrestricted = false
        });

        var permission = await repo.AddPermissionAsync(new Permission
        {
            Name = "MyPermission", Description = "MyPermission description."
        });

        await repo.AddPermissionToGroupAsync(group.Id, permission.Id);
        await repo.AddPlayerToGroupAsync(player.Id, group.Id);

        await repo.RemoveGroupAsync(group);

        var removedGroup = await dbFactory.GetConnection().GetTable<DbGroup>()
            .FirstOrDefaultAsync(g => g.Id == group.Id);
        
        Assert.Null(removedGroup);

        var removedGroupPermission = await dbFactory.GetConnection().GetTable<DbGroupPermission>()
            .FirstOrDefaultAsync(gp => gp.PermissionId == permission.Id);

        Assert.Null(removedGroupPermission);
        
        var removedUserGroup = await dbFactory.GetConnection().GetTable<DbUserGroup>()
            .FirstOrDefaultAsync(ug => ug.GroupId == group.Id); 
        
        Assert.Null(removedUserGroup);
    }

    [Fact]
    public async Task GetGroupAsync_Returns_Correct_Info()
    {
        var (repo, dbFactory) = CreateNewRepository();

        var addedGroup = await repo.AddGroupAsync(new Group
        {
            Title = "MyGroup",
            Description = "MyGroup description.",
            Icon = "A",
            Color = "aaa",
            Unrestricted = true
        });

        var group = await repo.GetGroupAsync(addedGroup.Id);
        
        Assert.NotNull(group);
        Assert.Equal("MyGroup", group.Title);
        Assert.Equal("MyGroup description.", group.Description);
        Assert.Equal("A", group.Icon);
        Assert.Equal("aaa", group.Color);
        Assert.True(group.Unrestricted);
    }

    [Fact]
    public async Task Player_Added_To_Group()
    {
        var (repo, dbFactory) = CreateNewRepository();

        var player = await DbTestHelper.AddTestPlayer(dbFactory);
        var group = await repo.AddGroupAsync(new Group {Title = "MyGroup", Description = "MyGroup description."});

        await repo.AddPlayerToGroupAsync(player.Id, group.Id);

        var userGroup = await dbFactory.GetConnection().GetTable<DbUserGroup>()
            .FirstOrDefaultAsync(r => r.UserId == player.Id && r.GroupId == group.Id);
        
        Assert.NotNull(userGroup);
    }

    [Fact]
    public async Task Player_Removed_From_Group()
    {
        var (repo, dbFactory) = CreateNewRepository();

        var player = await DbTestHelper.AddTestPlayer(dbFactory);
        var group = await repo.AddGroupAsync(new Group {Title = "MyGroup", Description = "MyGroup description."});

        await repo.AddPlayerToGroupAsync(player.Id, group.Id);
        await repo.RemovePlayerFromGroupAsync(player.Id, group.Id);

        var userGroup = await dbFactory.GetConnection().GetTable<DbUserGroup>()
            .FirstOrDefaultAsync(r => r.UserId == player.Id && r.GroupId == group.Id);
        
        Assert.Null(userGroup);
    }

    [Fact]
    public async Task Permission_Added_To_Group()
    {
        var (repo, dbFactory) = CreateNewRepository();
        
        var group = await repo.AddGroupAsync(new Group {Title = "MyGroup", Description = "MyGroup description."});
        var permission = await repo.AddPermissionAsync(new Permission
        {
            Name = "MyPermission", Description = "MyPermission description."
        });

        await repo.AddPermissionToGroupAsync(group.Id, permission.Id);

        var groupPermission = await dbFactory.GetConnection().GetTable<DbGroupPermission>()
            .FirstOrDefaultAsync(r => r.GroupId == group.Id && r.PermissionId == permission.Id);
        
        Assert.NotNull(groupPermission);
    }

    [Fact]
    public async Task Permission_Removed_From_Group()
    {
        var (repo, dbFactory) = CreateNewRepository();
        
        var group = await repo.AddGroupAsync(new Group {Title = "MyGroup", Description = "MyGroup description."});
        var permission = await repo.AddPermissionAsync(new Permission
        {
            Name = "MyPermission", Description = "MyPermission description."
        });

        await repo.AddPermissionToGroupAsync(group.Id, permission.Id);
        await repo.RemovePermissionFromGroupAsync(group.Id, permission.Id);

        var groupPermission = await dbFactory.GetConnection().GetTable<DbGroupPermission>()
            .FirstOrDefaultAsync(r => r.GroupId == group.Id && r.PermissionId == permission.Id);
        
        Assert.Null(groupPermission);
    }

    [Fact]
    public async Task All_Group_Permissions_Cleared()
    {
        var (repo, dbFactory) = CreateNewRepository();
        
        var group = await repo.AddGroupAsync(new Group {Title = "MyGroup", Description = "MyGroup description."});

        await repo.AddPermissionAsync(new Permission {Name = "P1", Description = "P1 Description."});
        await repo.AddPermissionAsync(new Permission {Name = "P2", Description = "P2 Description."});
        await repo.AddPermissionAsync(new Permission {Name = "P3", Description = "P3 Description."});

        await repo.ClearGroupPermissionsAsync(group.Id);

        var permissions = await dbFactory.GetConnection().GetTable<DbGroupPermission>()
            .Where(r => r.GroupId == group.Id).ToListAsync();
        
        Assert.Empty(permissions);
    }
}
