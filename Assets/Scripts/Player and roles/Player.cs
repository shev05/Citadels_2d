using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public int money;
    public RoleCard role;
    public int id;
    public string nickname;
    public int table = 1;
    public int numberTable;
    public bool isLocal;
    public bool isKing = false;
    public bool isActive = false;
    public bool haveUlt = true;
    public bool addMoney = true;
    public bool robbed = false;
    public bool isKill = false;
    public bool destructionAvaliable = true;
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
        money = 2;
        id = idPlayer;
        isLocal =  IsLocal;
        numberTable = number; 
    }

}