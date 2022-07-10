using System.Linq;
using System.Threading.Tasks;
using EvoSC.Domain;
using EvoSC.Domain.Groups;
using EvoSC.Interfaces.Commands;
using Microsoft.EntityFrameworkCore;

namespace EvoSC.Core.Services.Commands;

public class PermissionsService : IPermissionsService
{
    private readonly DatabaseContext _dbContext;

    public PermissionsService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddPermission(string name, string description)
    {
        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Name == name);

        if (permission == null)
        {
            var masterGroup = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Id == SystemGroups.MasterAdmin);
            permission = new Permission {Name = name, Description = description, Groups = new[] {masterGroup}};

            await _dbContext.Permissions.AddAsync(permission);
            await _dbContext.SaveChangesAsync();
        }
    }
}
