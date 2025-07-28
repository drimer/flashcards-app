using System.Globalization;

namespace FlashcardApp.Model;

public sealed class Pokemon : ITopic
{
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public string[] Types { get; set; } = [];
    public int Hp { get; set; }

    public string CapitalisedName => Name.Length > 0 ? char.ToUpper(Name[0], CultureInfo.InvariantCulture) + Name[1..] : string.Empty;
}
