using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlacerCScr : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int maxCards = 15;
    private List<CardMovementScr> placedCards = new List<CardMovementScr>();
    private CardMovementScr cardHovered = null;
    private string swappableField = "RoleField";
    public void OnDrop(PointerEventData eventData)
    {
        CardMovementScr card = eventData.pointerDrag.GetComponent<CardMovementScr>();
        DropPlacerCScr previousContainer = card.DefaultParent.GetComponent<DropPlacerCScr>();
        DropPlacerCScr targetContainer = GetComponent<DropPlacerCScr>();
        

        if ((previousContainer.CompareTag("HandField") && targetContainer.CompareTag("RoleCardField")) ||
             (previousContainer.CompareTag("RoleCardField") && (targetContainer.CompareTag("HandField") || 
                                                                targetContainer.CompareTag("PlayingField")))||
             (previousContainer.name == swappableField && targetContainer.CompareTag("RoleCardField")) ||
             previousContainer.CompareTag("PlayingField") && targetContainer.CompareTag("RoleCardField"))
        {
            Debug.Log(targetContainer.name + targetContainer.tag + placedCards.Count);
            return;
        }
        
        if (card)
        {
            if (previousContainer != null && previousContainer != this)
            {
                previousContainer.RemoveCardFromGroup(card);
            }
            
            if (placedCards.Count < maxCards)
            {
                // Добавляем карточку, если не превышен лимит
                AddCardToGroup(card);
                if (cardHovered == null && targetContainer.name == swappableField && previousContainer.CompareTag("RoleCardField"))
                {
                    //Debug.Log(card.name);
                    cardHovered = card;
                }
            }
            else if (cardHovered != null)
            {
                Debug.Log(cardHovered.name);
                SwapCards(card);
                Debug.Log(cardHovered.name);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null){
            return;
        }

        CardMovementScr card = eventData.pointerDrag.GetComponent<CardMovementScr>();

        if (card)
        {
            card.DefaultTempCardParent = transform;
            //cardHovered = card;
        }
        //Debug.Log(cardHovered.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null){
            return;
        }    

        CardMovementScr card = eventData.pointerDrag.GetComponent<CardMovementScr>();

        if (card && card.DefaultTempCardParent == transform)
        {
            card.DefaultTempCardParent = card.DefaultParent;
            //cardHovered = null;
        }
    }
    
    private void AddCardToGroup(CardMovementScr card)
    {
        card.DefaultParent = transform;
        placedCards.Add(card); // Добавляем карточку в список размещённых
        //Debug.Log("Объект добавлен в группу.");
    }
    
    private void RemoveCardFromGroup(CardMovementScr card)
    {
        if (placedCards.Contains(card))
        {
            placedCards.Remove(card); // Удаляем карточку из списка, если она там есть
            //Debug.Log("Объект удалён из группы.");
        }
    }
    
    private void SwapCards(CardMovementScr newCard)
    {
        // Убираем старую карточку из списка
        Transform newCardParent = newCard.DefaultParent;
        Transform hoveredCardParent = cardHovered.DefaultParent;

        // Запоминаем текущие позиции
        Vector3 newCardPosition = newCard.transform.localPosition;
        Vector3 hoveredCardPosition = cardHovered.transform.localPosition;

        // Меняем карточки местами
        newCard.transform.SetParent(hoveredCardParent);
        newCard.transform.localPosition = hoveredCardPosition;

        cardHovered.transform.SetParent(newCardParent);
        cardHovered.transform.localPosition = newCardPosition;

        // Обновляем родительский объект для каждой карточки
        newCard.DefaultParent = hoveredCardParent;
        cardHovered.DefaultParent = newCardParent;

        cardHovered = newCard;

        // Обновляем группы карточек в родителях
        if (newCardParent.GetComponent<DropPlacerCScr>())
        {
            newCardParent.GetComponent<DropPlacerCScr>().AddCardToGroup(cardHovered);
            newCardParent.GetComponent<DropPlacerCScr>().RemoveCardFromGroup(newCard);
        }

        if (hoveredCardParent.GetComponent<DropPlacerCScr>())
        {
            hoveredCardParent.GetComponent<DropPlacerCScr>().AddCardToGroup(newCard);
            hoveredCardParent.GetComponent<DropPlacerCScr>().RemoveCardFromGroup(cardHovered);
        }
    }
}
