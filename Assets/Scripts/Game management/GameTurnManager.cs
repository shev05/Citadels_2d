using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

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
    private CardDealer cardDealer;
    private SoundManager soundManager;
    public GameObject WinPanel;
    public List<TMP_Text> TextEnd;
    public GameObject ButtonSide;
    private bool isPanelVisible = true;
    public static int activePlayer = 0;
    public TMP_Text textButton;

    
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        _chooseRole = FindObjectOfType<ChooseRole>();
        playerState = FindObjectOfType<UpdatePlayerState>();
        cardDealer = FindObjectOfType<CardDealer>();
        soundManager = FindObjectOfType<SoundManager>();
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
        string roles = "";
        if(!isKill){
            Debug.Log(role);
            LocalizationHelper.GetLocalizedString("UI", role, text => 
            {
                roles = text;
                roleTexts[tableNumber].text = "<s>" + roles + "</s>";
            });
            var roleInfo = roleTexts[tableNumber].transform.parent.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            LocalizationHelper.GetLocalizedString("UI", role + "Text", text => 
            {
                roleInfo.text = "<s>" + text + "</s>";
            });           
            soundManager.AssassinKill();
            activePlayer++;

        }
        else{
            Debug.Log(role);
            LocalizationHelper.GetLocalizedString("UI", role, text => 
            {
                roles = text;
                roleTexts[tableNumber].text = roles;
            });
            var roleInfo = roleTexts[tableNumber].transform.parent.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            LocalizationHelper.GetLocalizedString("UI", role + "Text", text => 
            {
                roleInfo.text = text;
            });
            soundManager.RoleSound(role);
        }

    }

    public void ButtonNextTurn_Click(){
        photonView.RPC("SevenCards", RpcTarget.All);
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
            if(CheckedEnd()){
                _chooseRole.startChoosing();
            }
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
                WriteRole();
            }
    }
    private void WriteRole(){
        var player = turnBasedPlayerList[activePlayer];
        if(player.isKill){
            photonView.RPC("ShowRole", RpcTarget.All, player.role.Name, false);
            StartCoroutine(DelayBeforeNextTurn()); // Задержка перед переходом к следующему ходу
        }
        else{
            if(player.robbed)
                foreach(var p in turnBasedPlayerList){
                    if(p.role.Name == "Thief"){
                        //здесь
                        StartCoroutine(DelayBeforeRobbed(player.id, p.id, player.role.Name)); // Задержка перед краже
                    }
                }
            else{
                photonView.RPC("ShowRole", RpcTarget.All, player.role.Name, true);
                CheckedArchitect();
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
        var player = StartGame.players[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        if(player.haveLibrary)
            cardDealer.StartDealingWithCount(2);
        else{
        cardChoisePanel.SetActive(true);
        for (int i = 0; i < player.giveCardInStartTurn; i++){   
            int randomIndex = UnityEngine.Random.Range(0, CardDealer.deck.Count);
            Card selectedCard = CardDealer.deck[randomIndex];
            var cardObject = Instantiate(choiseCardPrefab, cardChoisePanel.transform.GetChild(0).transform, false);
            cardObject.GetComponent<CardInfoScr>().ShowCardInfo(selectedCard);
        }
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
        soundManager.NextTurnSound();    
        StartGame.round += 1;
        foreach(var player in StartGame.players){
            player.isKill = false; 
            player.robbed = false;
            player.haveUlt = true;
            player.addMoney = true;
            player.destructionAvaliable = true;
            player.placeableCardCount = 1;
            player.haveSmithyUlt = true;
            player.haveLaboratoryUlt = true;
        }
        foreach(var item in roleTexts)
        {
            item.text = "";
            var roleInfo = item.transform.parent.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            roleInfo.text = "";
        }
        foreach (var roleBut in ButtonRole)
            roleBut.SetActive(true);
        KillPlayer.roleNameKill = "";
                    
                
    }
    [PunRPC]
    void Robbed(int idRobPlayer, int idThiefPlayer){
        var players = StartGame.players;
        int moneyCount = players[idRobPlayer - 1].money;
        players[idRobPlayer - 1].money = 0;
        players[idThiefPlayer - 1].money += moneyCount;
        soundManager.ThiefSteal();
    }

    void CheckedArchitect(){
        var player = turnBasedPlayerList[activePlayer];
        if(player.role.Name == "Architect")
            player.placeableCardCount = 3;
    }
    [PunRPC]
    void SevenCards(){
        int index;
        if(activePlayer == 0)
            index = activePlayer;
        else 
            index = activePlayer - 1;
        if(turnBasedPlayerList[index].placedCards.Count >= 7){ //Кол-во карт для оканчивания игры
            bool first = true;
            foreach(var p in StartGame.players){
                if(p.isEndFirst){
                    turnBasedPlayerList[index].score += 2;
                    first = false;
                }
            }
            if(first){
                turnBasedPlayerList[index].score += 4;
                turnBasedPlayerList[index].isEndFirst = true;
            }
        }
    }
    bool CheckedEnd(){
        var players = StartGame.players;
        foreach(var player in players){
            if(player.isEndFirst){
                photonView.RPC("Scoring", RpcTarget.All);
                return false;
            }
        }
        return true;
    }

    [PunRPC]
    void Scoring(){
        var players = StartGame.players;
        List<int> idScore = new List<int>();
        foreach(var player in players){
            foreach(var card in player.placedCards){
                player.score += card.cost;
                if(card.Name == "Dragongate" || card.Name == "University")
                    player.score +=2;
            }
            if(CheckedAllTypes(player))
                player.score +=3;
            idScore.Add(player.score);
        }
        int index = idScore.IndexOf(idScore.Max());
        WinPanel.SetActive(true);
        if(players[PhotonNetwork.LocalPlayer.ActorNumber - 1].id == players[index].id)
            LocalizationHelper.GetLocalizedString("UI", "WinText", text => 
            {
                TextEnd[0].text = text;
            });
        else
            LocalizationHelper.GetLocalizedString("UI", "LoseText", text => 
            {
                TextEnd[0].text = text;
            });
        LocalizationHelper.GetLocalizedString("UI", "ScoreText", text => 
        {
            TextEnd[1].text = text + players[PhotonNetwork.LocalPlayer.ActorNumber - 1].score;
        });
        LocalizationHelper.GetLocalizedString("UI", "ScoreWinText", text => 
        {
            TextEnd[2].text = text + players[index].score;
        });
        LocalizationHelper.GetLocalizedString("UI", "RoundText", text => 
        {
            TextEnd[3].text = text + StartGame.round;
        });
        if(PhotonNetwork.LocalPlayer.ActorNumber == players[index].id)
            soundManager.WinSound();
        else soundManager.LoseSound();
        ButtonSide.SetActive(true);
        
    }
    bool CheckedAllTypes(Player player){
        List<int> color = new List<int>{0,0,0,0,0};
        bool townCheck = false;
        foreach(var card in player.placedCards){
            if (card.Name == "Hauntedcity")
                townCheck = true;
            else if (card.Color == "Yellow")
                color[0]+=1;
            else if (card.Color == "Green")
                color[1]+=1;
            else if (card.Color == "Blue")
                color[2]+=1;
            else if (card.Color == "Red")
                color[3]+=1;
            else
                color[4]+=1;
        }
        if(color.Contains(0) && townCheck)
            color[color.IndexOf(0)] += 1; 
        if(color.Contains(0))
            return false;  
        else 
            return true;  
    }
    private IEnumerator DelayBeforeNextTurn(){
        yield return new WaitForSeconds(3.0f); 
        ButtonNextTurn_Click();
    }

        // Метод-корутина для задержки перед вызовом Robbed
    private IEnumerator DelayBeforeRobbed(int robbedPlayerId, int thiefPlayerId, string role){
        photonView.RPC("Robbed", RpcTarget.All, robbedPlayerId, thiefPlayerId);
        playerState.UpdateMoney();
        yield return new WaitForSeconds(3.0f); 
        photonView.RPC("ShowRole", RpcTarget.All, role, true);
        CheckedArchitect();
        choisePanel.SetActive(true); 
    }
    public void Click_Button(){
        isPanelVisible = !isPanelVisible;
        WinPanel.SetActive(isPanelVisible);

        LocalizationHelper.GetLocalizedString("UI", isPanelVisible ? "ButtonHide" : "ButtonShow", localizedText =>
        {
            textButton.text = localizedText;
        });
    }
}