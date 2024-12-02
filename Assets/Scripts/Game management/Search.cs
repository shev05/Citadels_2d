using UnityEngine;
using UnityEngine.UI;
using TMPro; // Подключаем TextMeshPro
using System.Text.RegularExpressions;

public class SearchManager : MonoBehaviour
{
    public TMP_InputField searchField; // Поле ввода текста для поиска (TMP)
    public TMP_Text[] sectionTexts; // Массив текстов всех разделов (TMP)
    public GameObject[] sectionPanels; // Панели разделов
    public Color highlightColor = Color.yellow; // Цвет подсветки

    private string[] originalTexts; // Массив для сохранения исходных текстов

    void Start()
    {
        // Сохраняем исходный текст всех разделов
        originalTexts = new string[sectionTexts.Length];
        for (int i = 0; i < sectionTexts.Length; i++)
        {
            originalTexts[i] = sectionTexts[i].text;
        }
    }

    // Метод поиска по регулярному выражению
    public void Search()
    {
        string query = searchField.text;

        if (string.IsNullOrEmpty(query))
        {
            ResetHighlights();
            return;
        }

        Regex regex;
        try
        {
            regex = new Regex(query, RegexOptions.IgnoreCase);
        }
        catch
        {
            Debug.LogWarning("Некорректное регулярное выражение.");
            return;
        }

        // Поиск и подсветка текста во всех разделах
        for (int i = 0; i < sectionTexts.Length; i++)
        {
            HighlightMatches(sectionTexts[i], regex, i);
        }
    }

    // Подсветка совпадений
    private void HighlightMatches(TMP_Text textComponent, Regex regex, int index)
    {
        if (textComponent == null) return;

        string content = originalTexts[index]; // Берем оригинальный текст

        // Найти все совпадения и заменить их с тегом <color>
        string highlightedText = regex.Replace(content, match =>
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{match.Value}</color>";
        });

        textComponent.text = highlightedText; // Устанавливаем обновленный текст
    }

    // Сброс подсветки
    public void ResetHighlights()
    {
        for (int i = 0; i < sectionTexts.Length; i++)
        {
            if (sectionTexts[i] != null)
                sectionTexts[i].text = originalTexts[i];
        }
    }
}