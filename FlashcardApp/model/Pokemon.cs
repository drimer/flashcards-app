namespace FlashcardApp.Model;


public sealed class Pokemon : ITopic
{
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public string[] Types { get; set; } = Array.Empty<string>();
    public int Hp { get; set; }

    public string CapitalisedName { get => Name.Length > 0 ? char.ToUpper(Name[0]) + Name[1..] : string.Empty; }
}