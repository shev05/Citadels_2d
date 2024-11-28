using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private BackgroundMusic instance;
    void Start()
    {
        var audioSource = GetComponent<AudioSource>();
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

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
