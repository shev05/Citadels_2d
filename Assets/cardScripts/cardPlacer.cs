using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript1 : MonoBehaviour
{
    public Transform spawnArea;

    public List<GameObject> cards;
    // Start is called before the first frame update
    void Start()
    {
        //cards = NewBehaviourScript.deck;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<GameObject> Cards = new List<GameObject> ();
            Shuffle(cards);
            Vector3 position = GetRandomPosition();
            Instantiate(cards[0], position, Quaternion.identity);
        }
    }
    
    Vector3 GetRandomPosition()
    {
        float x = Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2);
        float y = Random.Range(spawnArea.position.y - spawnArea.localScale.y / 2, spawnArea.position.y + spawnArea.localScale.y / 2);
        return new Vector3(x, y, 0);
    }

    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            T temp = list[i];
            int r = Random.Range(i, n);
            list[i] = list[r];
            list[r] = temp;
        }
    }
}
