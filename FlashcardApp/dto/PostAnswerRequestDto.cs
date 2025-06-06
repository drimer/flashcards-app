using FlashcardApp.Controller;
using FlashcardApp.Model;

namespace FlashcardApp.Dto;

public class PostAnswerRequestDto
{
    public string Type { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty; // should be part of context object, to be generic
    public Pokemon Pokemon { get; set; } = new Pokemon(); // should be part of context object, to be generic
    public string Answer { get; set; } = string.Empty;
}