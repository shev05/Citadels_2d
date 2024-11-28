using UnityEngine;

public class ShowRoleInfo : MonoBehaviour
{
    private GameObject _currentRoleField;
    
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Выполняем Raycast
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            // Проверяем, имеет ли объект нужный тег
            if (hit.collider.CompareTag("RoleCardField"))
            {
                // Проверяем, есть ли компонент SpriteRenderer и активная переменная
                var role = hit.collider.gameObject;
                    // Если навели на новый объект
                    if (_currentRoleField != role)
                    {
                        ResetCurrentRoleField(); // Сброс предыдущего аватара
                        _currentRoleField = role;
                        role.transform.GetChild(1).gameObject.SetActive(true); // Меняем цвет
                    }
            }
        }
        else
        {
            // Сбрасываем выделение, если курсор ни на что не указывает
            ResetCurrentRoleField();
        }
    }

    private void ResetCurrentRoleField()
    {
        if (_currentRoleField != null)
        {
            _currentRoleField.transform.GetChild(1).gameObject.SetActive(false); // Возвращаем исходный цвет
            _currentRoleField = null;
        }
    }
}
