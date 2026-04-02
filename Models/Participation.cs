namespace ContestSystem.Models;

public class Participation
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ContestId { get; set; }

    public int Score { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.Now;
}