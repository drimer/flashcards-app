namespace FlashcardApp.Model;


public sealed class Pokemon
{
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public string[] Types { get; set; } = Array.Empty<string>();
}