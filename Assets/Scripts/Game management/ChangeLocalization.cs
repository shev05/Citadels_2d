using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ChangeLocalization : MonoBehaviour
{
    [SerializeField] TMP_Dropdown  dropdown;
    
    public void Button_RU(){
        ChangeLanguage("ru");
    }
    
    public void Button_EN(){
        ChangeLanguage("en");
    }
    
    public void ChangeLanguage(string languageCode)
    {
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(dropdown.options[dropdown.value].text.ToLower());

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
