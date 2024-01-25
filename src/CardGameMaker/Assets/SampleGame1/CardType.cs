using UnityEngine;

[CreateAssetMenu(menuName = "Card", order = -100)]
public class CardType : ScriptableObject
{
    [PreviewSprite] [SerializeField] private Sprite art;
    [SerializeField] private string cardName;
    [SerializeField] [TextArea(1, 12)] public string description;
    [SerializeField] private int numCopies;
    
    public bool HasDescription => !string.IsNullOrWhiteSpace(description);
    public string Name => cardName;
    public Sprite Art => art;
    public int NumCopies => numCopies;
}
