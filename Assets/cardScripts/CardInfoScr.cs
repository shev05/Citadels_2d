using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScr : MonoBehaviour
{
    public Card SelfCard;
    public Image Logo;
    private GameObject EnlargedCardImage;
    private Image cardImage;

    public void ShowCardInfo(Card card){
        SelfCard = card;
        //transform.gameObject.GetComponent<Sprite>() = card.Logo;
        Logo.sprite = card.Logo;
        Logo.preserveAspect = false;
    }

    private void Start()
    {
        EnlargedCardImage = GameObject.Find("ImageZoom");
        //ShowCardInfo(CardsManager.AllCards[transform.GetSiblingIndex()]);
    }
    
    void OnMouseEnter()
    {
        cardImage = EnlargedCardImage.GetComponent<Image>();
        if (cardImage != null && Logo != null)
        {
            // Копируем спрайт с текущей карты в увеличенное изображение
            cardImage.sprite = Logo.sprite;
            cardImage.preserveAspect = true;

            // Делаем увеличенную карту видимой
            cardImage.enabled = true;
        }
    }

    // Метод, который вызывается при выходе курсора с карты
    void OnMouseExit()
    {
        if (cardImage != null)
        {
            // Скрываем увеличенное изображение карты
            cardImage.enabled = false;
        }
    }
    

}
