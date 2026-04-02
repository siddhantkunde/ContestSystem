namespace ContestSystem.Models;

public class Question
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty; 
    // Single / Multi / TrueFalse

    public int ContestId { get; set; }

    public Contest Contest { get; set; }

    public List<Option> Options { get; set; } = new();
}