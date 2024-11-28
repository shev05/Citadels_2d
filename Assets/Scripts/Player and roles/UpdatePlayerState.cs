using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class UpdatePlayerState : MonoBehaviour
{
    private PhotonView photonView;
    private List<Player> players;
    public List<TMP_Text> cardCount;
    public List<TMP_Text> moneyCount;
    public List<GameObject> Crown;
    
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void UpdateCard(){
        photonView.RPC("UpdateCardCount", RpcTarget.All);
    }

    [PunRPC]
    void UpdateCardCount(){
        players = StartGame.players;
        foreach(var player in players)
            cardCount[player.numberTable].text = player.cards.Count.ToString();
    }

    public void UpdateMoney(){
        photonView.RPC("UpdateMoneyCount", RpcTarget.All);
    }

    [PunRPC]
    void UpdateMoneyCount(){
        players = StartGame.players;
        foreach(var player in players)
            moneyCount[player.numberTable].text = player.money.ToString();
    }
    public void UpdateKing(){
        photonView.RPC("UpdateKingVisible", RpcTarget.All);
    }

    [PunRPC]
    void UpdateKingVisible(){
        players = StartGame.players;
        foreach(var player in players)
            if(player.isKing)
                Crown[player.numberTable].SetActive(true);
            else
                Crown[player.numberTable].SetActive(false);
    }
}
