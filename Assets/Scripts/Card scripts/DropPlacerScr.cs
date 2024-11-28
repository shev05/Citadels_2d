using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlacerCScr : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int maxCards = 15;
    private List<CardMovementScr> placedCards = new List<CardMovementScr>();
    private CardMovementScr cardHovered = null;
    public GameObject rotatedCard;
    public GameObject simpleCard;
    public GameObject tempCard;
    public List<GameObject> hands;
    public GameObject cardDropField;
    public GameObject handField;
    List<Player> players;
    PhotonView photonView;
    private UpdatePlayerState playerState;
    private SoundManager soundManager;


    void Start(){
        photonView = GetComponent<PhotonView>();
        playerState = FindObjectOfType<UpdatePlayerState>();
        soundManager = FindObjectOfType<SoundManager >();
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
            if(targetContainer.gameObject.name.Equals("Hand 1 player") && CheckPlacedCards(chosenCard.Name)) return;
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
                else if (targetContainer.gameObject.name.Equals("Hand 1 player")){
                    if(chosenCard.Name == "Laboratory" || chosenCard.Name == "Smithy"){
                        card.gameObject.AddComponent<ClickSpecPurple>();
                    }
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
        }
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
        }
    }
    
    private void AddCardToGroup(CardMovementScr card)
    {
        card.DefaultParent = transform;
        placedCards.Add(card);
    }
    
    private void RemoveCardFromGroup(CardMovementScr card)
    {
        if (placedCards.Contains(card))
        {
            placedCards.Remove(card);
        }
    }

    [PunRPC]
    void CardDroped(int indexPlayer, int cost, int cardIndex){
        players = StartGame.players;
        players[indexPlayer].placeableCardCount -= 1;
        players[indexPlayer].placedCards.Add(players[indexPlayer].cards[cardIndex]);
        Debug.Log(players[indexPlayer].nickname + " placed " + players[indexPlayer].cards[cardIndex].Name + " card");
        if(PhotonNetwork.LocalPlayer.ActorNumber == (indexPlayer + 1))
            if(players[indexPlayer].cards[cardIndex].Color == "Purple")
                CheckedPassivePurple(players[indexPlayer], players[indexPlayer].cards[cardIndex]);
        players[indexPlayer].cards.RemoveAt(cardIndex);
        players[indexPlayer].money -= cost;
        soundManager.BuildedSound();
    }
    
    [PunRPC]
    void DisplayCardOnOtherTables(int indexPlayer, int cardIndex)
    {
        players = StartGame.players;
        GameObject otherTableCard;
        var originalCard = players[indexPlayer].cards[cardIndex];
        
        if(players[indexPlayer].numberTable == 2) {
            otherTableCard = Instantiate(simpleCard);
            var script = otherTableCard.GetComponent<CardMovementScr>();
            Destroy(script);
        }
        else otherTableCard = Instantiate(rotatedCard);
        otherTableCard.GetComponent<CardInfoScr>().ShowCardInfo(originalCard);

        Transform otherPlayerTableTransform = hands[players[indexPlayer].numberTable].transform;
        otherTableCard.transform.SetParent(otherPlayerTableTransform);
        
        otherTableCard.transform.localPosition = Vector3.zero;
        otherTableCard.transform.localScale = new Vector3(1, 1, 1);
        if (players[indexPlayer].numberTable == 2) otherTableCard.transform.rotation = Quaternion.Euler(0, 0, 180);
        if (players[indexPlayer].numberTable == 3) otherTableCard.transform.rotation = Quaternion.Euler(0, 0, 180);
        
    }

    
    void CheckedPassivePurple(Player player, Card card)
    {
        Debug.Log(player.nickname + "`s " + card.Name + " is now active");
        if(card.Name =="Hauntedcity")
            player.roundForTown = StartGame.round;
        else if(card.Name == "Observatory")
            player.giveCardInStartTurn = 3;
        else if(card.Name == "Library")
            player.haveLibrary = true;
        else if(card.Name == "Graveyard")
            player.hasGraveyard = true;
    }

    private bool CheckPlacedCards(string name){
        var _players = StartGame.players;
        return _players[PhotonNetwork.LocalPlayer.ActorNumber - 1].placedCards.Any(card => card.Name == name);
    }
}
