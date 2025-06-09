using FlashcardApp.Model;

namespace FlashcardApp.Dto;

public class NewQuestionResponseDto
{
    public QuestionDto Question { get; set; } = new QuestionDto();
}