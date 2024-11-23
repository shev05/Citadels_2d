using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleChoosingScr : MonoBehaviour
{
    public GameObject popupPanel; // Ссылка на Panel всплывающего окна
    private KeyCode activationKey = KeyCode.Space;
    
    
    void Start()
    {
        popupPanel.SetActive(false); // Скрываем окно по умолчанию
    }
    
    void Update()
    {
        // Проверяем, нажата ли клавиша
        if (Input.GetKeyDown(activationKey))
        {
            TogglePopup(); // Показываем или скрываем окно
        }
    }
    
    void TogglePopup()
    {
        // Переключаем видимость панели
        popupPanel.SetActive(!popupPanel.activeSelf);
    }
}
