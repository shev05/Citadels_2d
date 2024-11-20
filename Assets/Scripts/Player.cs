using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class Player : MonoBehaviour
{
    //List <Player> players = new List<Player>();
    public int money;
    public RoleCard role;
    // List <Card> cards = new List<Card>();
    public int id;
    public int table = 1;
    public int numberTable;
    public bool isLocal;
    public bool isKing = false;
    public bool isActive = false;
    public bool haveUlt = true;
    public bool addMoney = true;
    public bool robbed = false;
    public bool isKill = false;
    public bool isEndFirst = false;
    public int score = 0;
    public int roundForTown = 100;
    public int giveCardInStartTurn = 2; 
    public bool haveLibrary = false;
    public bool haveSmithyUlt = true;
    public bool haveLaboratoryUlt = true;
    public bool hasGraveyard = false;
    

    public int placeableCardCount = 1;
    public List<Card> cards = new List<Card>();
    public List<Card> placedCards = new List<Card>();


    public Player (int idPlayer, bool IsLocal, int number)
    {
        money = 25;
        id = idPlayer;
        isLocal =  IsLocal;
        numberTable = number; 
    }

}