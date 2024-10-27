using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class StartGame : MonoBehaviour
{
    static public List<Player> players;
    private PhotonView photonView;
    [SerializeField] Button button;
    [SerializeField] TMP_Text[] money; 
    [SerializeField] TMP_Text[] cardCount; 

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    public void SendButton()
    {
        photonView.RPC("start", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void start(){
        RemoveButton();
        players = new List<Player>();
                foreach (var player in PhotonNetwork.PlayerList){
                    if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber)
                        players.Add(new Player(player.ActorNumber, true));
                    else
                        players.Add(new Player(player.ActorNumber, false));
            }
        foreach (var m in money)
            m.text = "2";
        foreach (var c in cardCount)
            c.text = "4";
    }

    void RemoveButton(){
        Destroy(button.gameObject);
    }
}
