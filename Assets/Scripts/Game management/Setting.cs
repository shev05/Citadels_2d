using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField] Slider masterSlider;  // Привязать слайдер для MasterVolume
    [SerializeField] Slider musicSlider;   // Привязать слайдер для MusicVolume
    [SerializeField] Slider sfxSlider;     // Привязать слайдер для SFXVolume

    private void Start()
    {
        // Установить начальные значения слайдеров из текущих настроек
        if (SoundManager.Instance != null)
        {
            masterSlider.value = SoundManager.Instance.masterVolume;
            musicSlider.value = SoundManager.Instance.musicVolume;
            sfxSlider.value = SoundManager.Instance.sfxVolume;
        }

    }

    public void ApplySettings()
    {
        if (SoundManager.Instance != null)
        {
            // Передать значения слайдеров в SoundManager
            SoundManager.Instance.SetMasterVolume(masterSlider.value);
            SoundManager.Instance.SetMusicVolume(musicSlider.value);
            SoundManager.Instance.SetSFXVolume(sfxSlider.value);
            Debug.Log("Настройки громкости применены!");

        }

    }
}
