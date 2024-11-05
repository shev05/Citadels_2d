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
    public bool robbed = false;
    public bool isKill = false;

    public int placeableCardCount = 1;
    public List<Card> cards = new List<Card>();


    public Player (int idPlayer, bool IsLocal, int number)
    {
        money = 2;
        id = idPlayer;
        isLocal =  IsLocal;
        numberTable = number; 
    }

}
