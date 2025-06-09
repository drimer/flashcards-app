using FlashcardApp.Controller;
using FlashcardApp.Model;

namespace FlashcardApp.Dto;

public class PostAnswerRequestDto
{
    public QuestionDto Question { get; set; } = new QuestionDto();
    public string Answer { get; set; } = string.Empty;
}