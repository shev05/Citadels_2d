using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class StartGame : MonoBehaviourPunCallbacks
{
    static public List<Player> players;
    static public int round;
    private PhotonView photonView;
    public List<TMP_Text> nicknames;
    private string[] nicks = new string[4];
    [SerializeField] Button button;
    [SerializeField] GameObject[] kings;
    private UpdatePlayerState playerState;
    private int playerNum = 0;


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
            photonView.RPC("W", RpcTarget.AllBuffered);
            playerState.UpdateMoney();
            playerState.UpdateCard();
        }
    }

    [PunRPC]
    void start(int randomIndex){
        RemoveButton();
        Debug.Log("Game started");
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

    [PunRPC]
    void SetNickName(string[] nickNames)
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].nickname = nickNames[i];
            nicknames[players[i].numberTable].text = nickNames[i];
        }
    }
    
    [PunRPC]
    public IEnumerator W()
    {
        foreach (var player in players)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == player.id)
            {
                string nickName = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("NickName")
                    ? PhotonNetwork.LocalPlayer.CustomProperties["NickName"].ToString()
                    : "Player";
                photonView.RPC("AddNickname", RpcTarget.All, nickName);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    [PunRPC]
    public void AddNickname(string nick)
    {
        players[playerNum].nickname = nick;
        nicknames[players[playerNum++].numberTable].text = nick;
    }
}