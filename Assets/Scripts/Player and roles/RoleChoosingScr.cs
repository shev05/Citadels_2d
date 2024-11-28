using UnityEngine;

public class RoleChoosingScr : MonoBehaviour
{
    public GameObject popupPanel;
    private KeyCode activationKey = KeyCode.Space;
    
    
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
    }
    
    void TogglePopup()
    {
        popupPanel.SetActive(!popupPanel.activeSelf);
    }
}
