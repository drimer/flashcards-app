using FlashcardApp.Model;

namespace FlashcardApp.Dto;

public class NewQuestionResponseDto
{
    public string? Error { get; set; }
    public QuestionDto[] Questions { get; set; } = Array.Empty<QuestionDto>();
}