using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class CardSelectionTest
{
    GameObject cardPrefab;
    // A Test behaves as an ordinary method

    [SetUp]
    public void Setup()
    {
        cardPrefab = Resources.Load<GameObject>("Assets/card");
        GameObject cameraObject = new GameObject("MainCamera");
        Camera camera = cameraObject.AddComponent<Camera>();
        cameraObject.tag = "MainCamera";
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator CardSelectionTestWithEnumeratorPasses()
    {
        GameObject cardObject;
        cardObject = Object.Instantiate(cardPrefab);
        cardObject.GetComponent<CardInfoScr>().ShowCardInfo(new Card("qwerty", "", "Red", 12));

        Card card = (Card)cardObject.GetComponent<CardInfoScr>().SelfCard;

        Assert.AreEqual("Red", card.Color);        
        yield return null;
    }

    [UnityTest]
    public IEnumerator CardSelectionTestWithEnumeratorFailes()
    {
        GameObject cardObject;
        cardObject = Object.Instantiate(cardPrefab);
        cardObject.GetComponent<CardInfoScr>().ShowCardInfo(new Card("qwerty", "", "Blue", 12));
        Card card = (Card)cardObject.GetComponent<CardInfoScr>().SelfCard;

        Assert.AreNotEqual("Red", card.Color);
        yield return null;
    }
}
