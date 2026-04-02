namespace ContestSystem.Models;

public class Answer
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public int SelectedOptionId { get; set; }

    public int ParticipationId { get; set; }
}