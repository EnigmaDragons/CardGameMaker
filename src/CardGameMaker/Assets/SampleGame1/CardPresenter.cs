using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPresenter : MonoBehaviour
{
    public string layout;
    public TextMeshProUGUI nameLabel;
    public GameObject nameLabelContainer;
    public Image cardArt;
    public TextMeshProUGUI descriptionLabel;
    public GameObject descriptionContainer;
    public TextMeshProUGUI numCopiesLabel;
    public GameObject numCopiesContainer;
    public Image typeIcon;
    public GameObject typeIconContainer;

    public void Activate(ICardType c)
    {
        Init(c);
        gameObject.SetActive(true);
    }
    
    public void Init(ICardType c)
    {
        cardArt.sprite = c.Art;
        if (nameLabel != null)
            nameLabel.text = c.Name;
        if (nameLabelContainer != null)
            nameLabelContainer.SetActive(true);
        if (descriptionLabel != null)
            descriptionLabel.text = c.Description;
        if (descriptionContainer != null)
            descriptionContainer.SetActive(c.HasDescription());
        if (numCopiesLabel != null)
            numCopiesLabel.text = c.NumCopies.ToString();
        if (numCopiesContainer != null)
            numCopiesContainer.SetActive(c.NumCopies > 1);
        if (typeIconContainer != null)
            typeIconContainer.SetActive(true);
    }
}
