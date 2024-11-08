using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class DestructionManager : MonoBehaviour
{
    private List<Player> _players;
    private PhotonView _photonView;
    public List<TMP_Text> _playerMoney;
    public GameObject clickedCard;
    public List<GameObject> hands;
    public GameObject prevCard;
    public GameObject destroyButton;
    
    // Start is called before the first frame update
    void Start()
    {
        prevCard = null;
        _photonView = GetComponent<PhotonView>();
    }

    public void Destruction()
    {
        _players = StartGame.players;
        var currentPlayer = _players[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        var chosenCard = (Card)clickedCard.GetComponent<CardInfoScr>().SelfCard;
        if (currentPlayer.money >= chosenCard.cost)
        {
            var handIndex = hands.IndexOf(FindParent(clickedCard));
            var cardIndex = clickedCard.transform.GetSiblingIndex();
            var playerIndex = _players.Find(player => player.numberTable == handIndex).id - 1;
            Debug.Log(handIndex + " qq " + cardIndex + " qq " + playerIndex);
            _photonView.RPC("UpdateMoney", RpcTarget.All, 
                currentPlayer.id, chosenCard.cost - 1);
            _photonView.RPC("DestroyCard", RpcTarget.All, handIndex, cardIndex, playerIndex);
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
    void UpdateMoney(int id, int cost)
    {
        _players = StartGame.players;
        var currentPlayer = _players[id - 1];
        currentPlayer.money -= cost;
        _playerMoney[currentPlayer.numberTable].text = currentPlayer.money.ToString();
    }

    [PunRPC]
    void DestroyCard(int handIndex, int cardIndex, int playerIndex)
    {
        // Находим объект по его PhotonView ID
        GameObject cardToDestroy = hands[_players[playerIndex].numberTable]
            .transform.GetChild(cardIndex).gameObject;
        
        if (cardToDestroy != null)
        {
            Destroy(cardToDestroy);
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

    public Player FindPlayer()
    {
        _players = StartGame.players;
        var handIndex = hands.IndexOf(FindParent(clickedCard));
        if (handIndex == -1) return null;
        return _players.Find(player => player.numberTable == handIndex);
    }
}
