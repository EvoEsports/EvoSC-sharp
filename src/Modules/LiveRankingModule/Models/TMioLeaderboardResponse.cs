namespace EvoSC.Modules.Official.LiveRankingModule.Models;

public class Parent
{
    public string name { get; set; }
    public string flag { get; set; }
    public Parent parent { get; set; }
}

public class Player
{
    public string name { get; set; }
    public string id { get; set; }
    public Zone zone { get; set; }
}

public class TMioLeaderboardResponse
{
    public List<Top> tops { get; set; }
    public int playercount { get; set; }
}

public class Top
{
    public Player player { get; set; }
    public int position { get; set; }
    public int time { get; set; }
    public string filename { get; set; }
    public DateTime timestamp { get; set; }
    public string url { get; set; }
}

public class Zone
{
    public string name { get; set; }
    public string flag { get; set; }
    public Parent parent { get; set; }
}
