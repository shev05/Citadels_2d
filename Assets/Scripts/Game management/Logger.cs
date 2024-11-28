using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger: MonoBehaviour
{
    private string logFilePath;

    void Awake()
    {
        // Задаем путь к файлу лога
        logFilePath = Path.Combine(Application.persistentDataPath, "Temp", "log.txt");

        // Устанавливаем наш кастомный логгер
        Debug.unityLogger.logHandler = new FileLogHandler(logFilePath);

        // Добавляем заголовок к новому файлу лога
        Debug.Log("=== Начало сессии логирования ===");
    }
}

public class FileLogHandler : ILogHandler
{
    private readonly ILogHandler defaultLogHandler = Debug.unityLogger.logHandler;
    private readonly string filePath;

    public FileLogHandler(string filePath)
    {
        this.filePath = filePath;
    }

    public void LogFormat(LogType logType, Object context, string format, params object[] args)
    {
        string message = string.Format(format, args);
        string logEntry = $"{System.DateTime.Now}: [{logType}] {message}\n";

        // Записываем сообщение в файл
        File.AppendAllText(filePath, logEntry);

        // Также отправляем сообщение в стандартный логгер Unity
        defaultLogHandler.LogFormat(logType, context, format, args);
    }

    public void LogException(System.Exception exception, Object context)
    {
        string logEntry = $"{System.DateTime.Now}: [Exception] {exception}\n";

        // Записываем исключение в файл
        File.AppendAllText(filePath, logEntry);

        // Также отправляем исключение в стандартный логгер Unity
        defaultLogHandler.LogException(exception, context);
    }
}
