using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlacerCScr : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int maxCards = 15;
    private List<CardMovementScr> placedCards = new List<CardMovementScr>();
    private CardMovementScr cardHovered = null;
    public GameObject rotatedCard;
    public List<GameObject> hands;
    private string swappableField = "RoleField";
    List<Player> players;
    public TMP_Text[] money_player;
    public TMP_Text[] card_player; 
    PhotonView photonView;

    void Start(){
        photonView = GetComponent<PhotonView>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        players = StartGame.players;
        int indexPlayer = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        CardMovementScr card = eventData.pointerDrag.GetComponent<CardMovementScr>();
        var chosenCard = (Card)eventData.pointerDrag.GetComponent<CardInfoScr>().SelfCard;
        int cardIndex = players[indexPlayer].cards.FindIndex(obj => obj == chosenCard);
        DropPlacerCScr previousContainer = card.DefaultParent.GetComponent<DropPlacerCScr>();
        DropPlacerCScr targetContainer = GetComponent<DropPlacerCScr>();
        if(!players[indexPlayer].isActive ||
         players[indexPlayer].placeableCardCount == 0 ||
         players[indexPlayer].money < chosenCard.cost)
            return;
        
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
                players[indexPlayer].placeableCardCount -= 1;
                photonView.RPC("DisplayCardOnOtherTables", RpcTarget.Others, indexPlayer,
                    cardIndex);
                photonView.RPC("CardDroped", RpcTarget.All, indexPlayer, chosenCard.cost, cardIndex);
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

    [PunRPC]
    void CardDroped(int indexPlayer, int cost, int cardIndex){
        players = StartGame.players;
        players[indexPlayer].cards.RemoveAt(cardIndex);
        players[indexPlayer].money -= cost;
        money_player[players[indexPlayer].numberTable].text = players[indexPlayer].money.ToString();
        card_player[players[indexPlayer].numberTable].text = players[indexPlayer].cards.Count.ToString();
    }
    
    [PunRPC]
    void DisplayCardOnOtherTables(int indexPlayer, int cardIndex)
    {
        players = StartGame.players;
        GameObject otherTableCard;
        // Получаем оригинальную карту по индексу из списка игрока
        var originalCard = players[indexPlayer].cards[cardIndex];
        // Создаем отображение другой версии карты на столах других игроков
        otherTableCard = Instantiate(rotatedCard);
        // Настраиваем отображаемую карту
        otherTableCard.GetComponent<CardInfoScr>().ShowCardInfo(originalCard);

        // Определяем стол для размещения карты
        Transform otherPlayerTableTransform = hands[players[indexPlayer].numberTable].transform;

        // Настройка позиции и привязки к столу
        otherTableCard.transform.SetParent(otherPlayerTableTransform);
        otherTableCard.transform.localPosition = Vector3.zero;
        otherTableCard.transform.localScale = new Vector3(1, 1, 1);
        if (players[indexPlayer].numberTable == 2) otherTableCard.transform.rotation = Quaternion.Euler(0, 0, 90);
        if (players[indexPlayer].numberTable == 3) otherTableCard.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (players[indexPlayer].numberTable == 4) otherTableCard.transform.rotation = Quaternion.Euler(0, 0, 180);
    }
}
