using UnityEngine;

public interface ICardType
{
    string Layout { get; }
    string Back { get;  }
    string Name { get; }
    string Description { get; }
    string Type { get; }
    string TypePrefix { get; }
    int NumCopies { get; }
    
    Sprite Art { get; }
    Sprite Icon { get; }
}

public static class CardTypeExtensions
{
    public static bool HasDescription(this ICardType c) => !string.IsNullOrWhiteSpace(c.Description);
}
