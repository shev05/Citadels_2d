using UnityEngine;

public class HandShow : MonoBehaviour
{
    [SerializeField] GameObject popupPanel;
    [SerializeField] GameObject settingPanel;
    [SerializeField] GameObject manualPanel;

    private KeyCode activationKey = KeyCode.Space;
    private KeyCode activationKeyEsc = KeyCode.Escape;
    private KeyCode activationTab = KeyCode.Tab;

    void Start()
    {
        popupPanel.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            TogglePopup();
        }
        else if (Input.GetKeyDown(activationKeyEsc))
        {
            SettingsActive();
        }
        else if (Input.GetKeyDown(activationTab))
        {
            ManualActive();
        }
    }
    
    void TogglePopup()
    {
        popupPanel.SetActive(!popupPanel.activeSelf);
    }
    void SettingsActive()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    void ManualActive()
    {
        manualPanel.SetActive(!manualPanel.activeSelf);
    }
}
