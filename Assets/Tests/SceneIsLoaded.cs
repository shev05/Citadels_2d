using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class SceneIsLoaded
{
    private const string MainMenuSceneName = "MainMenu"; // Название начальной сцены
    private const string GameSceneName = "Menu";   // Название сцены, которая должна загрузиться
    private Button button;

    [UnityTest]
    public IEnumerator SceneIsLoadedWithEnumeratorPasses()
    {
        // Загружаем начальную сцену
        var loadOp = SceneManager.LoadSceneAsync(MainMenuSceneName);
        yield return new WaitUntil(() => loadOp.isDone);

        // Проверяем, что начальная сцена загружена
        Assert.AreEqual(MainMenuSceneName, SceneManager.GetActiveScene().name, "Main menu scene not loaded");

        // Находим кнопку на сцене
        var buttonObject = GameObject.Find("ButtonPlay");
        Assert.IsNotNull(buttonObject, "Button not found on MainMenu scene");

        button = buttonObject.GetComponent<Button>();
        Assert.IsNotNull(button, "Button component not found");

        // Нажимаем кнопку программно
        button.onClick.Invoke();

        // Ждем загрузки новой сцены
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == GameSceneName);

        // Проверяем, что загрузилась нужная сцена
        Assert.AreEqual(GameSceneName, SceneManager.GetActiveScene().name, "Game scene not loaded");
    }
    
    [UnityTest]
    public IEnumerator SceneIsLoadedWithEnumeratorFailure()
    {
        // Загружаем начальную сцену
        var loadOp = SceneManager.LoadSceneAsync(MainMenuSceneName);
        yield return new WaitUntil(() => loadOp.isDone);

        // Проверяем, что начальная сцена загружена
        Assert.AreEqual(MainMenuSceneName, SceneManager.GetActiveScene().name, "Main menu scene not loaded");

        // Находим кнопку на сцене
        var buttonObject = GameObject.Find("ButtonSettings");
        Assert.IsNotNull(buttonObject, "Button not found on MainMenu scene");

        button = buttonObject.GetComponent<Button>();
        Assert.IsNotNull(button, "Button component not found");

        // Нажимаем кнопку программно
        button.onClick.Invoke();

        // Проверяем, что загрузилась нужная сцена
        Assert.AreNotEqual(GameSceneName, SceneManager.GetActiveScene().name, "Game scene not loaded");
    }
}
