using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource; // Источник для музыки
    [SerializeField] AudioSource sfxSource;   // Источник для звуковых эффектов

    [Header("Audio Clips")]
    [SerializeField] AudioClip cardDealer;
    [SerializeField] AudioClip cardPut;
    [SerializeField] AudioClip buildCard;
    [SerializeField] AudioClip assassinCard;
    [SerializeField] AudioClip thiefCard;
    [SerializeField] AudioClip magicianCard;
    [SerializeField] AudioClip kingCard;
    [SerializeField] AudioClip bishopCard;
    [SerializeField] AudioClip merchantCard;
    [SerializeField] AudioClip architectCard;
    [SerializeField] AudioClip warlordCard;
    [SerializeField] AudioClip assassinKill;
    [SerializeField] AudioClip thiefSteal;
    [SerializeField] AudioClip magicianSwap;
    [SerializeField] AudioClip warlordDestroy;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;
    [SerializeField] AudioClip nextTurn;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем между сценами
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadVolumeSettings(); // Загружаем настройки громкости при старте
    }

    #region Volume Management

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        SaveVolumeSetting("MasterVolume", masterVolume);
        ApplyVolume();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        SaveVolumeSetting("MusicVolume", musicVolume);
        ApplyVolume();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        SaveVolumeSetting("SFXVolume", sfxVolume);
        ApplyVolume();
    }

    private void ApplyVolume()
    {
        // Настраиваем громкость для музыки и эффектов
        if (musicSource != null)
            musicSource.volume = masterVolume * musicVolume;

        if (sfxSource != null)
            sfxSource.volume = masterVolume * sfxVolume;
    }

    private void LoadVolumeSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        ApplyVolume();
    }

    private void SaveVolumeSetting(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    #endregion

    #region Sound Playback

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource != null)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void PlayEffect(AudioClip clip, float volumeScale = 1f)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
        }
    }

    #endregion

    #region Predefined Sounds

    public void CardDealerSound() => PlayEffect(cardDealer);
    public void CardPutSound() => PlayEffect(cardPut);
    public void BuildedSound() => PlayEffect(buildCard, 0.5f);

    public void RoleSound(string name)
    {
        switch (name)
        {
            case "Assassin": PlayEffect(assassinCard, 0.25f); break;
            case "Thief": PlayEffect(thiefCard); break;
            case "Magician": PlayEffect(magicianCard, 0.75f); break;
            case "King": PlayEffect(kingCard); break;
            case "Bishop": PlayEffect(bishopCard, 0.5f); break;
            case "Merchant": PlayEffect(merchantCard, 0.75f); break;
            case "Architect": PlayEffect(architectCard, 0.75f); break;
            case "Warlord": PlayEffect(warlordCard); break;
        }
    }

    public void AssassinKill() => PlayEffect(assassinKill);
    public void ThiefSteal() => PlayEffect(thiefSteal);
    public void MagicianSwap() => PlayEffect(magicianSwap);
    public void WarlordDestroy() => PlayEffect(warlordDestroy);
    public void WinSound() => PlayEffect(winSound);
    public void LoseSound() => PlayEffect(loseSound);
    public void NextTurnSound() => PlayEffect(nextTurn);

    #endregion
}
