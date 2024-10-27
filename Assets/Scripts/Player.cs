using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class Player : MonoBehaviour
{
    //List <Player> players = new List<Player>();
    public int money;
    public string role;
   // List <Card> cards = new List<Card>();
    public int id;
    public bool isLocal;
    public List<GameObject> cards = new List<GameObject>();

    public Player (int idPlayer, bool IsLocal)
    {
        money = 2;
        id = idPlayer;
        isLocal =  IsLocal;
    }

}
