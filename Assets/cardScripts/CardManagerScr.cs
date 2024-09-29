using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public struct Card{
    public string Name;
    public string Color;
    public Sprite Logo;
    public int cost;

    public Card(string name, string logoPath,string color, int Cost){
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        cost = Cost;
        Color = color;
    }
}

public static class CardsManager
{
    public static List<Card> AllCards = new List<Card>();
}

public class CardManagerScr : MonoBehaviour
{
    void Awake(){
        CardsManager.AllCards.Add(new Card("docks", "picture/location/Green/docks", "Green", 3));
        CardsManager.AllCards.Add(new Card("Harbor", "picture/location/Green/harbor", "Green", 4));
        CardsManager.AllCards.Add(new Card("Market", "picture/location/Green/market", "Green", 2));
        CardsManager.AllCards.Add(new Card("docks", "picture/location/Green/docks", "Green", 3));
        CardsManager.AllCards.Add(new Card("Docks", "picture/location/Green/docks", "Green", 3));

        

      
        CardsManager.AllCards.Add(new Card("Market", "picture/location/Green/market", "Green", 2));
        CardsManager.AllCards.Add(new Card("Market", "picture/location/Green/market", "Green", 2));
        CardsManager.AllCards.Add(new Card("Market", "picture/location/Green/market", "Green", 2));

        CardsManager.AllCards.Add(new Card("Tavern", "picture/location/Green/tavern", "Green", 1));
        CardsManager.AllCards.Add(new Card("Townhall", "picture/location/Green/townhall", "Green", 5));

        CardsManager.AllCards.Add(new Card("TradingPost", "picture/location/Green/tradingpost", "Green", 2));
        CardsManager.AllCards.Add(new Card("TradingPost", "picture/location/Green/tradingpost", "Green", 2));
        CardsManager.AllCards.Add(new Card("TradingPost", "picture/location/Green/tradingpost", "Green", 2));
    }
  /*  public void AwakeBlue()
    {
        CardManager.AllCards.Add(new Card("Docks", "Assets/cards/location/picture/Green/docks.png", "Blue", 3));
    }

    public void AwakePurple()
    {

    }*/

    
}
