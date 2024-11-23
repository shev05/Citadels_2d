using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSpecPurple : MonoBehaviour
{
    private float lastClickTime = 0f; // Время последнего клика
    private float doubleClickThreshold = 0.3f; // Интервал для двойного клика в секундах
    PurpleActive purpleActive;

    void Start()
    {
        purpleActive = FindObjectOfType<PurpleActive>();
    }

    void Update()
    {
        // Проверяем, был ли клик мыши
        if (Input.GetMouseButtonDown(0)) // 0 - левая кнопка мыши
        {
            float currentTime = Time.time;
            if (currentTime - lastClickTime <= doubleClickThreshold)
            {
                TogglePopup(); // Если двойной клик
            }
            lastClickTime = currentTime;
        }
    }

    void TogglePopup()
    {
        purpleActive.ActiveCard(gameObject);
    }
}
