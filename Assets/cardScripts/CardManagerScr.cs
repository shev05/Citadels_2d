using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public struct Card{
    public string Name;
    public string Color;
    public Sprite Logo;
    public int cost;
    public string Ability;

    public Card(string name, string logoPath,string color, int Cost, string ability = ""){
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        cost = Cost;
        Color = color;
        Ability = ability;
    }
}

public static class CardsManager
{
    public static List<Card> AllCards = new List<Card>();
}

public class CardManagerScr : MonoBehaviour
{
    void Awake(){
        //green
        CardsManager.AllCards.Add(new Card("docks", "picture/location/Green/docks", "Green", 3));
        CardsManager.AllCards.Add(new Card("docks", "picture/location/Green/docks", "Green", 3));
        CardsManager.AllCards.Add(new Card("docks", "picture/location/Green/docks", "Green", 3));

        CardsManager.AllCards.Add(new Card("Harbor", "picture/location/Green/harbor", "Green", 4));
        CardsManager.AllCards.Add(new Card("Harbor", "picture/location/Green/harbor", "Green", 4));
        CardsManager.AllCards.Add(new Card("Harbor", "picture/location/Green/harbor", "Green", 4));

        CardsManager.AllCards.Add(new Card("Market", "picture/location/Green/market", "Green", 2));
        CardsManager.AllCards.Add(new Card("Market", "picture/location/Green/market", "Green", 2));
        CardsManager.AllCards.Add(new Card("Market", "picture/location/Green/market", "Green", 2));
        CardsManager.AllCards.Add(new Card("Market", "picture/location/Green/market", "Green", 2));

        CardsManager.AllCards.Add(new Card("Tavern", "picture/location/Green/tavern", "Green", 1));
        CardsManager.AllCards.Add(new Card("Tavern", "picture/location/Green/tavern", "Green", 1));
        CardsManager.AllCards.Add(new Card("Tavern", "picture/location/Green/tavern", "Green", 1));
        CardsManager.AllCards.Add(new Card("Tavern", "picture/location/Green/tavern", "Green", 1));
        CardsManager.AllCards.Add(new Card("Tavern", "picture/location/Green/tavern", "Green", 1));

        CardsManager.AllCards.Add(new Card("Townhall", "picture/location/Green/townhall", "Green", 5));
        CardsManager.AllCards.Add(new Card("Townhall", "picture/location/Green/townhall", "Green", 5));

        CardsManager.AllCards.Add(new Card("TradingPost", "picture/location/Green/tradingpost", "Green", 2));
        CardsManager.AllCards.Add(new Card("TradingPost", "picture/location/Green/tradingpost", "Green", 2));
        CardsManager.AllCards.Add(new Card("TradingPost", "picture/location/Green/tradingpost", "Green", 2));

        //red
        CardsManager.AllCards.Add(new Card("Watchtower", "picture/location/Red/watchtower", "Red", 1));
        CardsManager.AllCards.Add(new Card("Watchtower", "picture/location/Red/watchtower", "Red", 1));
        CardsManager.AllCards.Add(new Card("Watchtower", "picture/location/Red/watchtower", "Red", 1));

        CardsManager.AllCards.Add(new Card("Prison", "picture/location/Red/prison", "Red", 2));
        CardsManager.AllCards.Add(new Card("Prison", "picture/location/Red/prison", "Red", 2));
        CardsManager.AllCards.Add(new Card("Prison", "picture/location/Red/prison", "Red", 2));

        CardsManager.AllCards.Add(new Card("Battlefield", "picture/location/Red/battlefield", "Red", 3));
        CardsManager.AllCards.Add(new Card("Battlefield", "picture/location/Red/battlefield", "Red", 3));
        CardsManager.AllCards.Add(new Card("Battlefield", "picture/location/Red/battlefield", "Red", 3));

        CardsManager.AllCards.Add(new Card("Fortress", "picture/location/Red/fortress", "Red", 5));
        CardsManager.AllCards.Add(new Card("Fortress", "picture/location/Red/fortress", "Red", 5));

        //Yellow
        CardsManager.AllCards.Add(new Card("Manor", "picture/location/Yellow/manor", "Yellow", 3));
        CardsManager.AllCards.Add(new Card("Manor", "picture/location/Yellow/manor", "Yellow", 3));
        CardsManager.AllCards.Add(new Card("Manor", "picture/location/Yellow/manor", "Yellow", 3));
        CardsManager.AllCards.Add(new Card("Manor", "picture/location/Yellow/manor", "Yellow", 3));
        CardsManager.AllCards.Add(new Card("Manor", "picture/location/Yellow/manor", "Yellow", 3));

        CardsManager.AllCards.Add(new Card("Castle", "picture/location/Yellow/castle", "Yellow", 4));
        CardsManager.AllCards.Add(new Card("Castle", "picture/location/Yellow/castle", "Yellow", 4));
        CardsManager.AllCards.Add(new Card("Castle", "picture/location/Yellow/castle", "Yellow", 4));
        CardsManager.AllCards.Add(new Card("Castle", "picture/location/Yellow/castle", "Yellow", 4));

        CardsManager.AllCards.Add(new Card("Palace", "picture/location/Yellow/palace", "Yellow", 5));
        CardsManager.AllCards.Add(new Card("Palace", "picture/location/Yellow/palace", "Yellow", 5));
        CardsManager.AllCards.Add(new Card("Palace", "picture/location/Yellow/palace", "Yellow", 5));

        //blue
        CardsManager.AllCards.Add(new Card("Temple", "picture/location/Blue/temple", "Blue", 1));
        CardsManager.AllCards.Add(new Card("Temple", "picture/location/Blue/temple", "Blue", 1));
        CardsManager.AllCards.Add(new Card("Temple", "picture/location/Blue/temple", "Blue", 1));

        CardsManager.AllCards.Add(new Card("Church", "picture/location/Blue/church", "Blue", 2));
        CardsManager.AllCards.Add(new Card("Church", "picture/location/Blue/church", "Blue", 2));
        CardsManager.AllCards.Add(new Card("Church", "picture/location/Blue/church", "Blue", 2));

        CardsManager.AllCards.Add(new Card("Monastery", "picture/location/Blue/monastery", "Blue", 3));
        CardsManager.AllCards.Add(new Card("Monastery", "picture/location/Blue/monastery", "Blue", 3));
        CardsManager.AllCards.Add(new Card("Monastery", "picture/location/Blue/monastery", "Blue", 3));

        CardsManager.AllCards.Add(new Card("Cathedral", "picture/location/Blue/cathedral", "Blue", 5));
        CardsManager.AllCards.Add(new Card("Cathedral", "picture/location/Blue/cathedral", "Blue", 5));

        //purple
        CardsManager.AllCards.Add(new Card("Hauntedcity", "picture/location/Purple/hauntedcity", "Purple", 2, "AnyColorEndGame"));

        CardsManager.AllCards.Add(new Card("Keep", "picture/location/Purple/keep", "Purple", 3, "DontBreak"));
        CardsManager.AllCards.Add(new Card("Keep", "picture/location/Purple/keep", "Purple", 3, "DontBreak"));

        CardsManager.AllCards.Add(new Card("Graveyard", "picture/location/Purple/graveyard", "Purple", 5, "Corruption"));

        CardsManager.AllCards.Add(new Card("Laboratory", "picture/location/Purple/laboratory", "Purple", 5, "exchange"));
        
        CardsManager.AllCards.Add(new Card("Observatory", "picture/location/Purple/observatory", "Purple", 5, "AddCard"));

        CardsManager.AllCards.Add(new Card("Smithy", "picture/location/Purple/smithy", "Purple", 5, "BuyCards"));

        CardsManager.AllCards.Add(new Card("Greatwall", "picture/location/Purple/greatwall", "Purple", 6, "UpperCostCond"));

        CardsManager.AllCards.Add(new Card("Library", "picture/location/Purple/library", "Purple", 6, "TwoCards"));

        CardsManager.AllCards.Add(new Card("Schoolofmagic", "picture/location/Purple/schoolofmagic", "Purple", 6, "AnyColorMove"));

        CardsManager.AllCards.Add(new Card("Dragongate", "picture/location/Purple/dragongate", "Purple", 6, "AddScoreEndGame"));

        CardsManager.AllCards.Add(new Card("University", "picture/location/Purple/university", "Purple", 6, "AddScoreEndGame"));
    }
}
