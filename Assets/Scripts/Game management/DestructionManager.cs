using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DestructionManager : MonoBehaviour
{
    private List<Player> _players;
    private PhotonView _photonView;
    public List<GameObject> hands;
    public GameObject prevCard;
    public GameObject destroyButton;
    public GameObject graveyardPanel;
    private UpdatePlayerState playerState;
    private Card _resurrectedCard;
    public GameObject storage;
    private int graveyardPlayerId = -1;
    public GameObject cardPrefab;
    private SoundManager soundManager;
    
    // Start is called before the first frame update
    void Start()
    {
        prevCard = null;
        _photonView = GetComponent<PhotonView>();
        playerState = FindObjectOfType<UpdatePlayerState>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void Destruction(GameObject clickedCard)
    {
        _players = StartGame.players;
        var currentPlayer = _players[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        var chosenCard = (Card)clickedCard.GetComponent<CardInfoScr>().SelfCard;
        if (currentPlayer.money >= chosenCard.cost)
        {
            var handIndex = hands.IndexOf(FindParent(clickedCard));
            var cardIndex = clickedCard.transform.GetSiblingIndex();
            var playerIndex = _players.Find(player => player.numberTable == handIndex).id - 1;
            _photonView.RPC("ResurrectCard", RpcTarget.All, cardIndex, playerIndex);
            _photonView.RPC("DestroyCard", RpcTarget.All, cardIndex, playerIndex,
                PhotonNetwork.LocalPlayer.ActorNumber - 1);
            playerState.UpdateMoney();
            prevCard = null;
            clickedCard = null;
            destroyButton.SetActive(false);
        }
        else
        {
            Debug.Log("Нет деняк");
        }
    }

    [PunRPC]
    void DestroyCard(int cardIndex, int playerIndex, int warlordIndex)
    {
        _players = StartGame.players;
        // Находим объект по его PhotonView ID
        GameObject cardToDestroy = hands[_players[playerIndex].numberTable]
            .transform.GetChild(cardIndex).gameObject;
        Card card = (Card)cardToDestroy.GetComponent<CardInfoScr>().SelfCard;
        _players[warlordIndex].money -= card.cost - 1;
        if (HasGreatWall(hands[_players[playerIndex].numberTable]
                .transform)) _players[warlordIndex].money -= 1;
        _players[playerIndex].placedCards.Remove((Card)cardToDestroy.GetComponent<CardInfoScr>().SelfCard);
        if (cardToDestroy != null)
        {
            if(card.Color.Equals("Purple")) RemoveCardEffect(playerIndex, card.Name);
            _players[playerIndex].placedCards.Remove(card);
            Destroy(cardToDestroy);
            soundManager.WarlordDestroy();
        }
        else
        {
            Debug.LogWarning("Карта не найдена для удаления.");
        }
    }

    GameObject FindParent(GameObject child)
    {
        foreach (var item in hands)
        {
            if (child.transform.IsChildOf(item.transform)) return item;
        }

        return null;
    }

    public Player FindPlayer(GameObject clickedCard)
    {
        _players = StartGame.players;
        var handIndex = hands.IndexOf(FindParent(clickedCard));
        if (handIndex == -1) return null;
        return _players.Find(player => player.numberTable == handIndex);
    }
    
    public bool HasGreatWall(Transform hand)
    {
        return hand.GetComponentsInChildren<CardInfoScr>() // Получаем все компоненты MyComponent
            .Any(component => component.SelfCard.Name == "Greatwall");
    }

    [PunRPC]
    void ResurrectCard(int cardIndex, int playerIndex){
        _players = StartGame.players;
        graveyardPlayerId = -1;
        _resurrectedCard = null;
        GameObject cardToDestroy = hands[_players[playerIndex].numberTable]
            .transform.GetChild(cardIndex).gameObject;
        Card card = (Card)cardToDestroy.GetComponent<CardInfoScr>().SelfCard;
        graveyardPanel.transform.GetChild(2).GetComponent<Image>().sprite = card.Logo;
        _resurrectedCard = card;
        foreach(var player in _players){
            if (player.hasGraveyard && player.money >= 1 && !player.role.Equals("Warlord"))
                graveyardPlayerId = player.id;
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == graveyardPlayerId)
                graveyardPanel.SetActive(true);
    }

    public void GetResurrectedCard(){
        GameObject card = Instantiate(cardPrefab, storage.transform);
        card.GetComponent<CardInfoScr>().ShowCardInfo(_resurrectedCard);
        _photonView.RPC("GetCard", RpcTarget.All, graveyardPlayerId);
        playerState.UpdateMoney();
        playerState.UpdateCard();
    }

    [PunRPC]
    void GetCard(int id){
        _players = StartGame.players;
        _players[id - 1].cards.Add(_resurrectedCard);
        _players[id - 1].money -= 1;
    }
    
    void RemoveCardEffect(int index, string name){
        var player = _players[index];
        if(name =="Hauntedcity")
            player.roundForTown = 100;
        else if(name == "Observatory")
            player.giveCardInStartTurn = 2;
        else if(name == "Library")
            player.haveLibrary = false;
        else if(name == "Graveyard")
            player.hasGraveyard = false;
    }
}
