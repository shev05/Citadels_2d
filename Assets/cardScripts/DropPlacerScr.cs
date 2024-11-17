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
    public GameObject tempCard;
    public List<GameObject> hands;
    public GameObject cardDropField;
    public GameObject handField;
    List<Player> players;
    PhotonView photonView;
    private UpdatePlayerState playerState;


    void Start(){
        photonView = GetComponent<PhotonView>();
        playerState = FindObjectOfType<UpdatePlayerState>();
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
        DropPlacerCScr cardDropContainer = cardDropField.GetComponent<DropPlacerCScr>();
        if (!players[indexPlayer].isActive)
        {
            return;
        }

        if (targetContainer != cardDropContainer &&
            (players[indexPlayer].placeableCardCount == 0 ||
            players[indexPlayer].money < chosenCard.cost))
        {
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
                Debug.Log(targetContainer.gameObject.name);
                if(targetContainer.gameObject.name.Equals("DropField")){
                    AddCardToGroup(card);
                }
                else{
                    AddCardToGroup(card);
                
                    photonView.RPC("DisplayCardOnOtherTables", RpcTarget.Others, indexPlayer,
                        cardIndex);
                    photonView.RPC("CardDroped", RpcTarget.All, indexPlayer, chosenCard.cost, cardIndex);
                    playerState.UpdateCard();
                    playerState.UpdateMoney();
                }
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

    [PunRPC]
    void CardDroped(int indexPlayer, int cost, int cardIndex){
        players = StartGame.players;
        players[indexPlayer].placeableCardCount -= 1;
        players[indexPlayer].cards.RemoveAt(cardIndex);
        players[indexPlayer].money -= cost;
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
        if (players[indexPlayer].numberTable == 3) otherTableCard.transform.rotation = Quaternion.Euler(0, 0, 180);
        
    }
}
