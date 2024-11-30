using UnityEngine;

public class HandShow : MonoBehaviour
{
    public GameObject popupPanel;
    public GameObject settingPanel;

    private KeyCode activationKey = KeyCode.Space;
    private KeyCode activationKeyEsc = KeyCode.Escape;

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
    }
    
    void TogglePopup()
    {
        popupPanel.SetActive(!popupPanel.activeSelf);
    }
    void SettingsActive()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }
}
