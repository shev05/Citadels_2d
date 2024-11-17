using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTurnManager : MonoBehaviour
{
    private List<Player> _players;
    private List<Player> turnBasedPlayerList;
    public List<TMP_Text> roleTexts;
    private PhotonView photonView;
    public Button nextTurnButton;
    public GameObject choisePanel;
    public GameObject cardChoisePanel;
    public GameObject choiseCardPrefab;
    private ChooseRole _chooseRole;
    public GameObject KillPanel;
    private int tableNumber;
    public List<GameObject> ButtonRole; 
    private UpdatePlayerState playerState;

    public static int activePlayer = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        _chooseRole = FindObjectOfType<ChooseRole>();
        playerState = FindObjectOfType<UpdatePlayerState>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTurn()
    {
        _players = StartGame.players;
        turnBasedPlayerList = _players.OrderBy(turn => turn.role.TurnNum).ToList();
        photonView.RPC("ZeroActivePlayer", RpcTarget.All);
        TurnStep();
    }

    [PunRPC]
    void ShowRole(string role, bool isKill){
        if(!isKill){
            roleTexts[tableNumber].text = "<s>" + role + "</s>";
            activePlayer++;
        }
        else
            roleTexts[tableNumber].text = role;

    }

    public void ButtonNextTurn_Click(){
        Debug.Log(activePlayer + "ББ");

        if (activePlayer >= turnBasedPlayerList.Count)
        {
            var players = StartGame.players;
            foreach (var player in players)
                if(player.role.Name == "King" )
                    foreach (var playerKing in players)
                        if(playerKing.isKing){
                            if(player.id == playerKing.id)
                                break;
                            else
                                photonView.RPC("SwapKing", RpcTarget.All, player.id, playerKing.id);
                        }
            playerState.UpdateKing();
            photonView.RPC("PassiveBool", RpcTarget.All);
            _chooseRole.startChoosing();
            nextTurnButton.gameObject.SetActive(false);
            return;
        }
        photonView.RPC("TurnStep", RpcTarget.All);
        nextTurnButton.gameObject.SetActive(false);
        turnBasedPlayerList[activePlayer - 1].isActive = false;
    }

    [PunRPC]
    private void SwapKing(int idNewKing, int idOldKing){
        var players = StartGame.players;
        players[idNewKing - 1].isKing = true;
        players[idOldKing - 1].isKing = false;
    }

    [PunRPC]
    private void TurnStep(){  
        var player = turnBasedPlayerList[activePlayer];
        tableNumber = player.numberTable;
            if(player.id == PhotonNetwork.LocalPlayer.ActorNumber){
                if(player.isKill){
                    photonView.RPC("ShowRole", RpcTarget.All, player.role.Name, false);
                    ButtonNextTurn_Click();
                }
                else{
                    if(player.robbed)
                        foreach(var p in turnBasedPlayerList){
                            if(p.role.Name == "Thief"){
                                photonView.RPC("Robbed", RpcTarget.All, player.id, p.id);
                                playerState.UpdateMoney();
                                }
                    }
                    CheckedArchitect();
                    photonView.RPC("ShowRole", RpcTarget.All, player.role.Name, true);
                    choisePanel.SetActive(true);    
                }
            }
    }

    [PunRPC]
    private void ChooseMoney(){
        var player = turnBasedPlayerList[activePlayer++];
        player.money += 2;
        player.isActive = true;
    }

    private void ChooseCard(){
        choisePanel.SetActive(false);
        cardChoisePanel.SetActive(true);
        for (int i = 0; i <= 1; i++){   
            int randomIndex = UnityEngine.Random.Range(0, CardDealer.deck.Count);
            Card selectedCard = CardDealer.deck[randomIndex];
            var cardObject = Instantiate(choiseCardPrefab, cardChoisePanel.transform.GetChild(0).transform, false);
            cardObject.GetComponent<CardInfoScr>().ShowCardInfo(selectedCard);
        }

        
    }

    public void MoneyButton_Click(){
        photonView.RPC("ChooseMoney", RpcTarget.All);
        playerState.UpdateMoney();
        choisePanel.SetActive(false);
        nextTurnButton.gameObject.SetActive(true);

    }

    public void CardButton_Click(){
        ChooseCard();
    }

    [PunRPC]
    public void ZeroActivePlayer()
    {
        activePlayer = 0;
    }

    [PunRPC]
    void PassiveBool(){
        foreach(var player in StartGame.players){
            player.isKill = false; 
            player.robbed = false;
            player.haveUlt = true;
            player.addMoney = true;
            player.placeableCardCount = 1;
        }
        foreach (GameObject roleBut in ButtonRole)
            roleBut.gameObject.SetActive(true);
                    
                
    }
    [PunRPC]
    void Robbed(int idRobPlayer, int idThiefPlayer){
        var players = StartGame.players;
        int moneyCount = players[idRobPlayer - 1].money;
        players[idRobPlayer - 1].money = 0;
        players[idThiefPlayer - 1].money += moneyCount;
    }

    void CheckedArchitect(){
        var player = turnBasedPlayerList[activePlayer];
        if(player.role.Name == "Architect")
            player.placeableCardCount = 3;
    }
}
