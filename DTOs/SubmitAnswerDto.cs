namespace ContestSystem.DTOs;

public class SubmitAnswerDto
{
    public int QuestionId { get; set; }

    public List<int> SelectedOptionIds { get; set; } = new();
}