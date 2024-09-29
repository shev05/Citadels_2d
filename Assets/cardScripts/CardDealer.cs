using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDealer : MonoBehaviour
{
    public Transform deckPosition;  // Позиция колоды (место, откуда "вылетают" карты)
    public Transform handPosition;   // Позиция руки (куда карты будут перемещаться)
    public int cardsToDeal = 5;      // Количество карт, которые нужно выдать
    public float animationDuration = 0.5f; // Длительность анимации переноса карты
    public GameObject cardPrefab;    // Префаб карты для создания объектов
    private List<Card> deck;         // Колода карт
    public Canvas canvas;
    private Vector3 screenCardPos;

    private void Start()
    {
        Vector3 worldPosition = deckPosition.position;
        screenCardPos = Camera.main.WorldToScreenPoint(worldPosition);

        deck = new List<Card>(CardsManager.AllCards);
    }

    private void Update()
    {
        // Проверяем нажатие клавиши (например, пробел)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            /*foreach (var d in deck)
            {
                Debug.Log(d.Name);
            }*/
            // Начинаем выдавать карты при нажатии пробела
            StartCoroutine(DealCards());
        }
    }

    // Функция для выдачи карт
    private IEnumerator DealCards()
    {
        // Проверяем, что карт в колоде достаточно
        if (deck.Count >= cardsToDeal)
        {
            for (int i = 0; i < cardsToDeal; i++)
            {
                // Выбираем случайную карту из колоды
                int randomIndex = Random.Range(0, deck.Count);
                Card selectedCard = deck[randomIndex];
                Debug.Log(selectedCard.Name);
                // Удаляем карту из колоды
                deck.RemoveAt(randomIndex);

                // Создаем объект карты в позиции колоды
                GameObject cardObject = Instantiate(cardPrefab, screenCardPos, Quaternion.identity);
                cardObject.transform.SetParent(canvas.transform, false);
                cardObject.GetComponent<CardInfoScr>().ShowCardInfo(selectedCard); // Отображаем информацию карты
                
                

                // Анимация перемещения карты в руку
                StartCoroutine(MoveCardToHand(cardObject, handPosition));

                // Задержка перед выдачей следующей карты
                yield return new WaitForSeconds(animationDuration / 2);
            }
        }
        else
        {
            Debug.LogWarning("Не хватает карт в колоде для выдачи.");
        }
    }

    // Короутина для анимации перемещения карты в руку
    private IEnumerator MoveCardToHand(GameObject card, Transform targetParent)
    {
        float elapsedTime = 0;
        Vector3 startingPosition = card.transform.position;

        // Анимируем перемещение карты к позиции руки
        while (elapsedTime < animationDuration)
        {
            card.transform.position = Vector3.Lerp(startingPosition, targetParent.position, (elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Убедимся, что карта точно находится в нужной позиции
        card.transform.position = targetParent.position;

        // Привязываем карту к родителю с HorizontalLayoutGroup
        card.transform.SetParent(targetParent);

        // Принудительно обновляем Layout после добавления новой карты
        LayoutRebuilder.ForceRebuildLayoutImmediate(targetParent.GetComponent<RectTransform>());
    }
}
