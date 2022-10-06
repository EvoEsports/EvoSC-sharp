using EvoSC.Common.Database.Models;

namespace EvoSC.Common.Database;

public class EvoScDb : Dapper.Database<EvoScDb>
{
    public Table<DbPlayer> Players { get; set; }
}
