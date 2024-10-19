using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CardDealer : MonoBehaviourPun
{
    public Transform[] handPositions;  // Массив с позициями рук для каждого игрока
    public int cardsToDeal = 5;        // Количество карт для раздачи
    public float animationDuration = 0.5f; // Длительность анимации перемещения карты
    public GameObject cardPrefab;      // Префаб карты для создания объектов
    private List<Card> deck;           // Колода карт
    public Canvas canvas;
    private Vector3 screenCardPos;
    private PhotonView photonView;

    private void Start()
    {
        Vector3 worldPosition = handPositions[0].position; // Позиция первой руки
        screenCardPos = Camera.main.WorldToScreenPoint(worldPosition);
        photonView = GetComponent<PhotonView>();

        // Создаем колоду
        deck = new List<Card>(CardsManager.AllCards);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            photonView.RPC("Deal", RpcTarget.All);
        }
    }

    [PunRPC]
    public void Deal()
    {
        StartCoroutine(DealCards());
    }

    // Функция для раздачи карт
    public IEnumerator DealCards()
    {
        if (deck.Count >= cardsToDeal)
        {
            for (int i = 0; i < cardsToDeal; i++)
            {
                // Выбираем случайную карту из колоды
                int randomIndex = Random.Range(0, deck.Count);
                Card selectedCard = deck[randomIndex];

                // Удаляем карту из колоды
                deck.RemoveAt(randomIndex);

                // Создаем объект карты
                GameObject cardObject = Instantiate(cardPrefab, screenCardPos, Quaternion.identity);
                cardObject.transform.SetParent(canvas.transform, false);
                cardObject.GetComponent<CardInfoScr>().ShowCardInfo(selectedCard);  // Отображаем информацию о карте

                // Определяем, в какую руку положить карту в зависимости от номера игрока
                int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // Индекс игрока (0 для первого, 1 для второго и т.д.)

                if (playerIndex >= 0 && playerIndex < handPositions.Length)
                {
                    StartCoroutine(MoveCardToHand(cardObject, handPositions[playerIndex]));
                }

                // Задержка перед выдачей следующей карты
                yield return new WaitForSeconds(animationDuration / 2);
            }
        }
        else
        {
            Debug.LogWarning("Не хватает карт в колоде для раздачи.");
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

        // Привязываем карту к родителю
        card.transform.SetParent(targetParent);

        // Обновляем Layout для корректного размещения карт
        LayoutRebuilder.ForceRebuildLayoutImmediate(targetParent.GetComponent<RectTransform>());
    }
}
