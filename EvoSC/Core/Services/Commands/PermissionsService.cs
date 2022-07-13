using System.Linq;
using System.Threading.Tasks;
using EvoSC.Domain;
using EvoSC.Domain.Groups;
using EvoSC.Interfaces.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Core.Services.Commands;

public class PermissionsService : IPermissionsService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PermissionsService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task AddPermission(string name, string description)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        
        var permission = await dbContext.Permissions.FirstOrDefaultAsync(p => p.Name == name);

        if (permission == null)
        {
            var masterGroup = await dbContext.Groups.FirstOrDefaultAsync(g => g.Id == SystemGroups.MasterAdmin);
            permission = new Permission {Name = name, Description = description, Groups = new[] {masterGroup}};

            await dbContext.Permissions.AddAsync(permission);
            await dbContext.SaveChangesAsync();
        }
    }
}
