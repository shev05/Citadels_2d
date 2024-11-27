using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ChangeLocalization : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
