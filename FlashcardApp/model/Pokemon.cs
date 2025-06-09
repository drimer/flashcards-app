namespace FlashcardApp.Model;


public sealed class Pokemon : ITopic
{
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public string[] Types { get; set; } = Array.Empty<string>();
}