using UnityEngine;

public abstract class BaseCard{
    public string Name;
    public Sprite Logo;
    public string Ability;

    public BaseCard(string name, string logoPath, string ability = ""){
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Ability = ability;
    }
}

public class BaseCardManager : MonoBehaviour
{
    
}
