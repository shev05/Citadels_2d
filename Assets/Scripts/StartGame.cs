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
    [SerializeField] GameObject[] kings;

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
        int numberTable = 1;
        players = new List<Player>();
                foreach (var player in PhotonNetwork.PlayerList){
                    if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber)
                        players.Add(new Player(player.ActorNumber, true, 0));
                    else
                        players.Add(new Player(player.ActorNumber, false, numberTable++));
            }
        foreach (var m in money)
            m.text = "2";
        foreach (var c in cardCount)
            c.text = "4";
        int randomIndex = UnityEngine.Random.Range(0, players.Count);
        players[randomIndex].isKing = true;
        Debug.Log("3245" + PhotonNetwork.LocalPlayer.ActorNumber);
        Debug.Log("wlkdefndmk" + randomIndex);
        foreach (var player in players)
        {
            Debug.Log(player.id + " " + player.numberTable);
        }

        kings[players[randomIndex].numberTable].SetActive(true);
    }



    void RemoveButton(){
        Destroy(button.gameObject);
    }
}
