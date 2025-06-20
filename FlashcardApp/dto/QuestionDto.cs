using FlashcardApp.Model;

namespace FlashcardApp.Dto;

public class QuestionDto
{
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public TopicDto? Topic { get; set; } = null;  // TODO: This should be using an interface or base class for generic topics
    public string Field { get; set; } = string.Empty; // should be part of context object, to be generic

}