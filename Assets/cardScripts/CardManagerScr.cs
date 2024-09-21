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
        CardsManager.AllCards.Add(new Card("docks", "picture/location/Green/docks", "Green", 3));
       // CardsManager.AllCards.Add(new Card("Docks", "location/picture/Green/docks", "Green", 3));

        //CardsManager.AllCards.Add(new Card("Harbor", "Assets/cards/location/picture/Green/harbor", "Green", 4));

        /*CardsManager.AllCards.Add(new Card("Market", "Assets/cards/location/picture/Green/market.png", "Green", 2));
        CardsManager.AllCards.Add(new Card("Market", "Assets/cards/location/picture/Green/market.png", "Green", 2));
        CardsManager.AllCards.Add(new Card("Market", "Assets/cards/location/picture/Green/market.png", "Green", 2));
        CardsManager.AllCards.Add(new Card("Market", "Assets/cards/location/picture/Green/market.png", "Green", 2));

        CardsManager.AllCards.Add(new Card("Tavern", "Assets/cards/location/picture/Green/tavern.png", "Green", 1));
        CardsManager.AllCards.Add(new Card("Townhall", "Assets/cards/location/picture/Green/townhall.png", "Green", 5));

        CardsManager.AllCards.Add(new Card("TradingPost", "Assets/cards/location/picture/Green/tradingpost.png", "Green", 2));
        CardsManager.AllCards.Add(new Card("TradingPost", "Assets/cards/location/picture/Green/tradingpost.png", "Green", 2));
        CardsManager.AllCards.Add(new Card("Tra dingPost", "Assets/cards/location/picture/Green/tradingpost.png", "Green", 2));*/
    }
  /*  public void AwakeBlue()
    {
        CardManager.AllCards.Add(new Card("Docks", "Assets/cards/location/picture/Green/docks.png", "Blue", 3));
    }

    public void AwakePurple()
    {

    }*/

    
}
