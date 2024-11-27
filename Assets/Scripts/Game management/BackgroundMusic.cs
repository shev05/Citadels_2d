using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private BackgroundMusic instance;
    // Start is called before the first frame update
    void Start()
    {
        var audioSource = GetComponent<AudioSource>();
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Awake()
    {
        // Если объект уже существует, уничтожаем дублирующий
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Устанавливаем объект как единственный экземпляр
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
