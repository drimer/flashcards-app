namespace FlashcardApp.Dto;

public class PostAnswerResponseDto
{
    public string? Error { get; set; }
    public bool IsCorrect { get; set; }
    public string Message { get; set; } = string.Empty;
}