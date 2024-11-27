using UnityEngine;
using UnityEngine.Localization;

public static class LocalizationHelper
{
    /// <summary>
    /// Возвращает локализованную строку с подстановкой аргументов.
    /// </summary>
    /// <param name="table">Название таблицы локализации.</param>
    /// <param name="key">Ключ локализованной строки.</param>
    /// <param name="callback">Метод, который будет вызван с результатом локализации.</param>
    /// <param name="arguments">Аргументы для подстановки в строку.</param>
    public static void GetLocalizedString(string table, string key, System.Action<string> callback, params object[] arguments)
    {
        LocalizedString localizedString = new LocalizedString { TableReference = table, TableEntryReference = key };

        if (arguments != null && arguments.Length > 0)
        {
            localizedString.Arguments = arguments;
        }

        localizedString.GetLocalizedStringAsync().Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                callback?.Invoke(handle.Result);
            }
            else
            {
                Debug.LogWarning($"Failed to load localized string: {key} from table: {table}");
                callback?.Invoke(string.Empty);
            }
        };
    }
}