using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.SetName.Interfaces;

public interface ISetNameService
{
    public Task SetNicknameAsync(IPlayer player, string newName);
}
