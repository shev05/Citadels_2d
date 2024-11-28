using UnityEngine;

public class ClickSpecPurple : MonoBehaviour
{
    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;
    PurpleActive purpleActive;

    void Start()
    {
        purpleActive = FindObjectOfType<PurpleActive>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float currentTime = Time.time;
            if (currentTime - lastClickTime <= doubleClickThreshold)
            {
                TogglePopup();
            }
            lastClickTime = currentTime;
        }
    }

    void TogglePopup()
    {
        purpleActive.ActiveCard(gameObject);
    }
}
