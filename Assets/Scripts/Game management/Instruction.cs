using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour
{
    [SerializeField] GameObject[] sectionPanels; // Массив панелей для разных разделов
    [SerializeField] Button[] sectionButtons; // Массив кнопок для выбора раздела

    void Start()
    {
        // Добавляем обработчики событий для кнопок
        for (int i = 0; i < sectionButtons.Length; i++)
        {
            int index = i; // Локальная переменная для индекса
            sectionButtons[i].onClick.AddListener(() => ShowSection(index));
        }

        // Скрываем все разделы при старте
        HideAllSections();
    }

    // Метод для отображения выбранного раздела
    void ShowSection(int index)
    {
        HideAllSections(); // Скрыть все разделы
        sectionPanels[index].SetActive(true); // Показать выбранный раздел
    }

    // Метод для скрытия всех разделов
    void HideAllSections()
    {
        foreach (var panel in sectionPanels)
        {
            panel.SetActive(false);
        }
    }
}