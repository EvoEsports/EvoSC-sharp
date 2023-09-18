namespace EvoSC.Modules.Official.LiveRankingModule.Models;

public class Parent
{
    public string Name { get; set; }
    public string Flag { get; set; }
    public Parent ParentObject { get; set; }
}

public class Player
{
    public string Name { get; set; }
    public string Id { get; set; }
    public Zone Zone { get; set; }
}

public class TMioLeaderboardResponse
{
    public List<Top> BestTimes { get; set; }
    public int PlayerCount { get; set; }
}

public class Top
{
    public Player Player { get; set; }
    public int Position { get; set; }
    public int Time { get; set; }
    public string Filename { get; set; }
    public DateTime Timestamp { get; set; }
    public string URL { get; set; }
}

public class Zone
{
    public string Name { get; set; }
    public string Flag { get; set; }
    public Parent ParentObject { get; set; }
}
