using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlayerStat : MonoBehaviour
{
    private GameObject _currentAvatar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Выполняем Raycast
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            // Проверяем, имеет ли объект нужный тег
            if (hit.collider.CompareTag("Avatar") || hit.collider.CompareTag("Player"))
            {
                // Проверяем, есть ли компонент SpriteRenderer и активная переменная
                var avatar = hit.collider.gameObject;
                    // Если навели на новый объект
                    if (_currentAvatar != avatar)
                    {
                        ResetCurrentAvatar(); // Сброс предыдущего аватара
                        _currentAvatar = avatar;
                        avatar.transform.GetChild(0).gameObject.SetActive(true); // Меняем цвет
                    }
            }
        }
        else
        {
            // Сбрасываем выделение, если курсор ни на что не указывает
            ResetCurrentAvatar();
        }
    }

    private void ResetCurrentAvatar()
    {
        if (_currentAvatar != null)
        {
            _currentAvatar.transform.GetChild(0).gameObject.SetActive(false); // Возвращаем исходный цвет
            _currentAvatar = null;
        }
    }
}
