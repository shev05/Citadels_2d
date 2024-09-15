using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public static List<GameObject> deck;
    // Start is called before the first frame update
    void Start()
    {
        deck = new List<GameObject>();
        deck.Add(Resources.Load<GameObject>("Assets/cards/characters/Assasin.prefab"));
        deck.Add(Resources.Load<GameObject>("Assets/cards/characters/thief.prefab"));
        deck.Add(Resources.Load<GameObject>("Assets/cards/characters/magician.prefab"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
