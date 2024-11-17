using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.AI;

public class UltScript : MonoBehaviour
{
    public GameObject KillPanel; // Ссылка на Panel всплывающего окна
    public GameObject ThiefPanel; // Ссылка на Panel всплывающего окна
    public GameObject MagicianPanel; // Ссылка на Panel всплывающего окна
    public List<GameObject> ButtonRole;
    public GameObject destructionPanel;
    public GameObject panel;

    private KeyCode activationKey = KeyCode.Q;
    private KeyCode activationKeyForMoney = KeyCode.M;
    PhotonView photonView;
    private CardDealer cardDealer;
    private UpdatePlayerState playerState;


    // Start is called before the first frame update
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
            }
            else if(players[indexPlayer].role.Name == "Architect"){
                cardDealer.StartDealingWithCount(2);
                players[indexPlayer].haveUlt = false;
            }
            else if(players[indexPlayer].role.Name == "Warlord")
                destructionPanel.SetActive(!destructionPanel.activeSelf); // Не готово, нужно предусмотреть, что карта точно выбрана
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
        if(addMoney != 0){
            photonView.RPC("AddMoneyPlayer", RpcTarget.All, addMoney, indexPlayer);
            playerState.UpdateMoney();
            }

    }

    [PunRPC] 
    void AddMoneyPlayer(int addMoney, int indexPlayer){
        var player = StartGame.players[indexPlayer];
        player.money += addMoney;
    }

    [PunRPC]
    void MerchantUlt(int id){
        var player = StartGame.players[id - 1];
        player.money += 1;
    }
 

}
