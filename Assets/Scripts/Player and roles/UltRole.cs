using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UltScript : MonoBehaviour
{
    [SerializeField] GameObject KillPanel;
    [SerializeField] GameObject ThiefPanel;
    [SerializeField] GameObject MagicianPanel;
    [SerializeField] List<GameObject> ButtonRole;
    [SerializeField] GameObject destructionPanel;
    [SerializeField] GameObject panel;

    private KeyCode activationKey = KeyCode.Q;
    private KeyCode activationKeyForMoney = KeyCode.M;
    private PhotonView photonView;
    private CardDealer cardDealer;
    private UpdatePlayerState playerState;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        cardDealer = FindObjectOfType<CardDealer>();
        playerState = FindObjectOfType<UpdatePlayerState>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(activationKey))
            Ult_Click(); // Показываем или скрываем окно
        
        if(Input.GetKeyDown(activationKeyForMoney)){
            Add_Money_Click();
        }

        
    }
    
    public void Ult_Click()
    {
        int indexPlayer = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var players = StartGame.players;
        if(players[indexPlayer].isActive && players[indexPlayer].haveUlt){
            if(players[indexPlayer].role.Name == "Assassin")
                KillPanel.SetActive(!KillPanel.activeSelf);

            else if(players[indexPlayer].role. Name == "Thief"){
                ThiefPanel.SetActive(!ThiefPanel.activeSelf);
                foreach (GameObject roleBut in ButtonRole)
                    if(roleBut.name == "Button"+KillPlayer.roleNameKill)
                        roleBut.gameObject.SetActive(false);    
            }
            else if (players[indexPlayer].role.Name == "Magician")
            {
                MagicianPanel.SetActive(!MagicianPanel.activeSelf);
            }
            else if (players[indexPlayer].role.Name == "Merchant")
            {
                photonView.RPC("MerchantUlt", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
                playerState.UpdateMoney();
                players[PhotonNetwork.LocalPlayer.ActorNumber - 1].haveUlt = false;
            }
            else if(players[indexPlayer].role.Name == "Architect"){
                cardDealer.StartDealingWithCount(2);
                players[indexPlayer].haveUlt = false;
            }
            else if(players[indexPlayer].role.Name == "Warlord")
                destructionPanel.SetActive(!destructionPanel.activeSelf);
        }
    }

    public void Add_Money_Click(){
        int indexPlayer = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var players = StartGame.players;
        if(players[indexPlayer].isActive && players[indexPlayer].addMoney){
            if(players[indexPlayer].role.Name == "King")
                Check_Card("Yellow", indexPlayer);
            else if(players[indexPlayer].role.Name == "Bishop")
                Check_Card("Blue", indexPlayer);
            else if(players[indexPlayer].role.Name == "Merchant")
                Check_Card("Green", indexPlayer);
            else if(players[indexPlayer].role.Name == "Warlord")
                Check_Card("Red", indexPlayer);
            players[indexPlayer].addMoney = false;
        }
    }

    private void Check_Card(string color, int indexPlayer){
        int addMoney = 0;
        foreach (Transform cardTransform in panel.transform){
            Card card = (Card)cardTransform.GetComponent<CardInfoScr>().SelfCard;
            if(card.Color == color || card.Name.Equals("Schoolofmagic"))
                addMoney++;
        }

        if (addMoney != 0)
        {
            photonView.RPC("AddMoneyPlayer", RpcTarget.All, addMoney, indexPlayer);
            playerState.UpdateMoney();
        }

    }

    [PunRPC] 
    void AddMoneyPlayer(int addMoney, int indexPlayer){
        var player = StartGame.players[indexPlayer];
        player.money += addMoney;
        Debug.Log(player.nickname + "got " + addMoney + " money");
    }

    [PunRPC]
    void MerchantUlt(int id)
    {
        var player = StartGame.players[id - 1];
        player.money += 1;
        Debug.Log("Merchant got extra coin");
    }
}
