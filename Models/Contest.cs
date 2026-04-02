namespace ContestSystem.Models;

public class Contest
{
    public int Id { get; set; }

    public string Name { get; set; } 

    public string AccessLevel { get; set; } // VIP / Normal

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
    public string Prize { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}