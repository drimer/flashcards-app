namespace FlashcardApp.Dto;

public class NewQuestionResponseDto
{
    public string Type { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty; // should be context object, to be generic
    public string Message { get; set; } = string.Empty;
}