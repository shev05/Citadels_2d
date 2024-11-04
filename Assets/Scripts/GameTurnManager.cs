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
    public List<TMP_Text> moneyCounters;
    public List<TMP_Text> roleTexts;
    private PhotonView photonView;
    public Button nextTurnButton;
    public GameObject choisePanel;
    public GameObject cardChoisePanel;
    public GameObject choiseCardPrefab;
    private ChooseRole _chooseRole;
    private int tableNumber;


    public static int activePlayer = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        _chooseRole = FindObjectOfType<ChooseRole>();
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
    void showRole(string role){
        roleTexts[tableNumber].text = role;
    }

    public void ButtonNextTurn_Click(){
        Debug.Log(activePlayer);
        if (activePlayer >= turnBasedPlayerList.Count)
        {
            activePlayer = 0;
            _chooseRole.startChoosing();
            nextTurnButton.gameObject.SetActive(false);
            return;
        }
        photonView.RPC("TurnStep", RpcTarget.All);
        nextTurnButton.gameObject.SetActive(false);
        turnBasedPlayerList[activePlayer - 1].isActive = false;
        turnBasedPlayerList[activePlayer - 1].placeableCardCount = 1;
    }

    [PunRPC]
    private void TurnStep(){
        
        var player = turnBasedPlayerList[activePlayer];
        tableNumber = player.numberTable;
            if(player.id == PhotonNetwork.LocalPlayer.ActorNumber){
                photonView.RPC("showRole", RpcTarget.All, player.role.Name);
                choisePanel.SetActive(true);
            }
    }

    [PunRPC]
    private void ChooseMoney(){
        var player = turnBasedPlayerList[activePlayer++];
        player.money += 2;
        moneyCounters[tableNumber].text = player.money.ToString();
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
}
