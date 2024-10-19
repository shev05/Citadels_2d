using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CardDealer : MonoBehaviourPun
{
    public Transform[] playerHandPositions;  // Массив с позициями рук для отображения у других игроков
    public Transform localHandPosition;      // Позиция руки для локального игрока (внизу экрана)
    public Transform tablePosition;          // Позиция стола для карт игрока, который запросил раздачу
    public int cardsToDeal = 5;              // Количество карт, которые нужно выдать
    public float animationDuration = 0.5f;   // Длительность анимации перемещения карты
    public GameObject cardPrefab;            // Префаб карты для создания объектов
    private List<Card> deck;                 // Колода карт
    public Canvas canvas;
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        // Создаем колоду карт
        deck = new List<Card>(CardsManager.AllCards);
        localHandPosition = playerHandPositions[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))  // Кнопка для запуска раздачи карт
        {
            photonView.RPC("StartDealing", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        

    }

    [PunRPC]
    public void StartDealing(int requestingPlayerID)
    {
        StartCoroutine(DealCards(requestingPlayerID));
    }

    public IEnumerator DealCards(int requestingPlayerID)
    {
        if (deck.Count >= cardsToDeal)
        {
            for (int i = 0; i < cardsToDeal; i++)
            {
                // Выбираем случайную карту из колоды
                int randomIndex = Random.Range(0, deck.Count);
                Card selectedCard = deck[randomIndex];
                deck.RemoveAt(randomIndex);

                // Создаем объект карты
                GameObject cardObject = Instantiate(cardPrefab, canvas.transform, false);
                cardObject.GetComponent<CardInfoScr>().ShowCardInfo(selectedCard); // Отображаем информацию карты

                // Проверяем, какой игрок запросил карты
                if (PhotonNetwork.LocalPlayer.ActorNumber == requestingPlayerID)
                {
                    // Если это локальный игрок, карты идут в его локальную руку
                    StartCoroutine(MoveCardToHand(cardObject, localHandPosition));
                }
                else  // Если это другой игрок, карты идут в его столовую позицию
                {
                    foreach (var player in PhotonNetwork.PlayerList)
                    {
                        if(player.ActorNumber == requestingPlayerID)
                            StartCoroutine(MoveCardToHand(cardObject, playerHandPositions[player.ActorNumber]));

                    }
                }

                // Задержка перед следующей картой
                yield return new WaitForSeconds(animationDuration / 2);
            }
        }
        else
        {
            Debug.LogWarning("Не хватает карт в колоде для раздачи.");
        }
    }

    private IEnumerator MoveCardToHand(GameObject card, Transform targetParent)
    {
        float elapsedTime = 0;
        Vector3 startingPosition = card.transform.position;

        while (elapsedTime < animationDuration)
        {
            card.transform.position = Vector3.Lerp(startingPosition, targetParent.position, (elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.transform.position = targetParent.position;
        card.transform.SetParent(targetParent);

        LayoutRebuilder.ForceRebuildLayoutImmediate(targetParent.GetComponent<RectTransform>());
    }
}

