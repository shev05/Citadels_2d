using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScr : MonoBehaviour
{
    public Card SelfCard;
    public Image Logo;

    public void ShowCardInfo(Card card){
        SelfCard = card;
        //transform.gameObject.GetComponent<Sprite>() = card.Logo;
        Logo.sprite = card.Logo;
        Logo.preserveAspect = false;
    }

    private void Start(){
        ShowCardInfo(CardsManager.AllCards[transform.GetSiblingIndex()]);
    }

}
