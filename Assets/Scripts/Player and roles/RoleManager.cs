using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoleCard : BaseCard{
    public int TurnNum;

    public RoleCard(string name, string logoPath, int turnNum, string ability):
     base(name, logoPath, ability){
        TurnNum = turnNum;
    }
}

public static class RoleCardsManager
{
    public static List<RoleCard> AllRoles = new List<RoleCard>();
}

public class RoleManager : MonoBehaviour
{  
    void Awake(){
        RoleCardsManager.AllRoles.Add(new RoleCard("Assassin", "picture/characters/assassin", 1, "Assassinate"));

        RoleCardsManager.AllRoles.Add(new RoleCard("Thief", "picture/characters/thief", 2, "BountyHunter"));
        
        RoleCardsManager.AllRoles.Add(new RoleCard("Magician", "picture/characters/magician", 3, "Swap"));
        
        RoleCardsManager.AllRoles.Add(new RoleCard("King", "picture/characters/king", 4, "Papich"));
        
        RoleCardsManager.AllRoles.Add(new RoleCard("Bishop", "picture/characters/bishop", 5, "Protected"));
        
        RoleCardsManager.AllRoles.Add(new RoleCard("Merchant", "picture/characters/merchant", 6, "ExtraMoney"));
        
        RoleCardsManager.AllRoles.Add(new RoleCard("Architect", "picture/characters/architect", 7, "ExtraBuild"));
        
        RoleCardsManager.AllRoles.Add(new RoleCard("Warlord", "picture/characters/warlord", 8, "Destroy"));

    }
}
