using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ChangeLocalization : MonoBehaviour
{
    public void Button_RU(){
        ChangeLanguage("ru");
    }
    public void Button_EN(){
        ChangeLanguage("en");
    }
    void ChangeLanguage(string languageCode)
    {
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(languageCode);

        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
            Debug.Log("Switched to: " + locale.LocaleName);
        }
        else
        {
            Debug.LogWarning("Locale not available: " + languageCode);
        }
    }
}
