using UnityEngine;
using UnityEngine.UI;

public class CardInfoScr : MonoBehaviour
{
    public BaseCard SelfCard;
    public Image Logo;
    private GameObject EnlargedCardImage;
    private Image cardImage;

    public void ShowCardInfo(BaseCard card){
        SelfCard = card;
        Logo.sprite = card.Logo;
        Logo.preserveAspect = false;
    }

    private void Start()
    {
        EnlargedCardImage = GameObject.Find("ImageZoom");
    }
    
    void OnMouseEnter()
    {
        cardImage = EnlargedCardImage.GetComponent<Image>();
        if (cardImage != null && Logo != null)
        {
            cardImage.sprite = Logo.sprite;
            cardImage.preserveAspect = true;
            cardImage.enabled = true;
        }
    }

    void OnMouseExit()
    {
        if (cardImage != null)
        {
            cardImage.enabled = false;
        }
    }
}
