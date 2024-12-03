using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class StartGame : MonoBehaviourPunCallbacks
{
    [SerializeField] List<TMP_Text> nicknames;
    [SerializeField] Button button;
    [SerializeField] GameObject[] kings;
    [SerializeField] private List<TMP_Text> gameCounts;
    
    private UpdatePlayerState playerState;
    private int playerNum = 0;
    private int playerNumCount = 0;
    private DBManager _dbManager;
    static public List<Player> players;
    static public int round;
    private PhotonView photonView;
    private string[] nicks = new string[4];



    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerState = FindObjectOfType<UpdatePlayerState>();
        _dbManager = FindObjectOfType<DBManager>();
    }

    public void SendButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int randomIndex = UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length);
            photonView.RPC("start", RpcTarget.AllBuffered, randomIndex);
            photonView.RPC("GetAllNicknames", RpcTarget.AllBuffered);
            photonView.RPC("GetAllGameCount", RpcTarget.AllBuffered);
            playerState.UpdateMoney();
            playerState.UpdateCard();
        }
    }

    [PunRPC]
    void start(int randomIndex)
    {
        RemoveButton();
        Debug.Log("Game started");
        int numberPlayer = PhotonNetwork.LocalPlayer.ActorNumber;
        int numberTable;
        players = new List<Player>();

        if (numberPlayer == 1)
        {
            numberTable = 1;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber)
                    players.Add(new Player(player.ActorNumber, true, 0));
                else
                    players.Add(new Player(player.ActorNumber, false, numberTable++));
            }
        }
        else if (numberPlayer == 2)
        {
            numberTable = 3;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber)
                    players.Add(new Player(player.ActorNumber, true, 0));
                else
                {
                    if (numberTable == 4)
                        numberTable = 1;
                    players.Add(new Player(player.ActorNumber, false, numberTable++));
                }
            }
        }
        else if (numberPlayer == 3)
        {
            numberTable = 2;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber)
                    players.Add(new Player(player.ActorNumber, true, 0));
                else
                {
                    if (numberTable == 4)
                        numberTable = 1;
                    players.Add(new Player(player.ActorNumber, false, numberTable++));
                }
            }
        }
        else
        {
            numberTable = 1;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber)
                    players.Add(new Player(player.ActorNumber, true, 0));
                else
                {
                    if (numberTable == 4)
                        numberTable = 1;
                    players.Add(new Player(player.ActorNumber, false, numberTable++));
                }
            }
        }

        players[randomIndex].isKing = true;
        kings[players[randomIndex].numberTable].SetActive(true);
    }



    void RemoveButton()
    {
        Destroy(button.gameObject);
    }

    [PunRPC]
    void GetGameCount(int count)
    {
        gameCounts[players[playerNumCount++].numberTable].text += " " + count;
    }

    [PunRPC]
    public IEnumerator GetAllGameCount()
    {
        foreach (var player in players)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == player.id)
            {
                photonView.RPC("GetGameCount", RpcTarget.All, _dbManager.GetAllStat().Item1.Count);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }


    [PunRPC]
    public IEnumerator GetAllNicknames()
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