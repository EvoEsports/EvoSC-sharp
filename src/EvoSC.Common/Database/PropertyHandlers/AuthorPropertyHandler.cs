using EvoSC.Common.Interfaces.Models;
using RepoDb.Interfaces;
using RepoDb.Options;

namespace EvoSC.Common.Database.PropertyHandlers;

public class AuthorPropertyHandler : IPropertyHandler<long, IPlayer>
{
    public IPlayer Get(long input, PropertyHandlerGetOptions options)
    {
        throw new NotImplementedException();
    }

    public long Set(IPlayer input, PropertyHandlerSetOptions options)
    {
        throw new NotImplementedException();
    }
}
