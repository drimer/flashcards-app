namespace FlashcardApp.Dto;

public class NewQuestionResponseDto
{
    public string Type { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
}