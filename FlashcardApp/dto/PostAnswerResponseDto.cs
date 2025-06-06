namespace FlashcardApp.Dto;

public class PostAnswerResponseDto
{
    public bool IsCorrect { get; set; }
    public string Message { get; set; } = string.Empty;
}