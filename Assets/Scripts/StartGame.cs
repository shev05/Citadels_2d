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
    [SerializeField] GameObject[] kings;
    private UpdatePlayerState playerState;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerState = FindObjectOfType<UpdatePlayerState>();

    }
    public void SendButton()
    {
        if (PhotonNetwork.IsMasterClient) 
        {
            int randomIndex = UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length);
            photonView.RPC("start", RpcTarget.AllBuffered, randomIndex);
            playerState.UpdateMoney();
            playerState.UpdateCard();
        }
    }

    [PunRPC]
    void start(int randomIndex){
        RemoveButton();
        int numberPlayer = PhotonNetwork.LocalPlayer.ActorNumber;
        int numberTable;
        players = new List<Player>();

        if(numberPlayer == 1){
            numberTable = 1;
            foreach (var player in PhotonNetwork.PlayerList){
                    if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber)
                        players.Add(new Player(player.ActorNumber, true, 0));
                    else
                        players.Add(new Player(player.ActorNumber, false, numberTable++));
            }
        }
        else if(numberPlayer == 2){
            numberTable = 3;
                    foreach (var player in PhotonNetwork.PlayerList){
                        if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber)
                            players.Add(new Player(player.ActorNumber, true, 0));
                        else{
                            if(numberTable == 4)
                                numberTable = 1;
                            players.Add(new Player(player.ActorNumber, false, numberTable++));
                        }
            }
        }
        else if(numberPlayer == 3){
            numberTable = 2;
                    foreach (var player in PhotonNetwork.PlayerList){
                        if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber)
                            players.Add(new Player(player.ActorNumber, true, 0));
                        else{
                            if(numberTable == 4)
                                numberTable = 1;
                            players.Add(new Player(player.ActorNumber, false, numberTable++));
                        }
            }
        }
        else {
            numberTable = 1;
                    foreach (var player in PhotonNetwork.PlayerList){
                        if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber)
                            players.Add(new Player(player.ActorNumber, true, 0));
                        else{
                            if(numberTable == 4)
                                numberTable = 1;
                            players.Add(new Player(player.ActorNumber, false, numberTable++));
                        }
            }
        }
        players[randomIndex].isKing = true;
        kings[players[randomIndex].numberTable].SetActive(true);
    }



    void RemoveButton(){
        Destroy(button.gameObject);
    }
}
