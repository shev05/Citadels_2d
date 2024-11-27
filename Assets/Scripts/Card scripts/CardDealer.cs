using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Unity.VisualScripting;
using TMPro;
using System.Threading;

public class CardDealer : MonoBehaviourPun
{
    public Transform playerHandPositions;  // Массив с позициями рук для отображения у других игроков
    public Transform tablePosition;          // Позиция стола для карт игрока, который запросил раздачу
    public Transform cardParent;
    public int cardsToDeal = 4;              // Количество карт, которые нужно выдать
    public float animationDuration = 0.5f;   // Длительность анимации перемещения карты
    public GameObject cardPrefab;            // Префаб карты для создания объектов
    public GameObject cardPref;            // Префаб карты для создания объектов
    List<Player> players;
    GameObject cardObject;
    public static List<Card> deck;                 // Колода карт
    public Canvas canvas;
    private PhotonView photonView;
    private UpdatePlayerState playerState;
    private SoundManager soundManager;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerState = FindObjectOfType<UpdatePlayerState>();
        soundManager = FindObjectOfType<SoundManager>();
        // Создаем колоду карт
        deck = new List<Card>(CardsManager.AllCards);
    }

    public void ClickStartButton(){
            photonView.RPC("StartDealing", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
    }
    
    public void StartDealingWithCount(int count){
        for(int i = 0; i < count; i++){
            IssuingCards(PhotonNetwork.LocalPlayer.ActorNumber);
        }
        playerState.UpdateCard();
    }


    [PunRPC]
    public void StartDealing(int requestingPlayerID)
    {
        StartCoroutine(DealCards(requestingPlayerID));
    }

    public IEnumerator DealCards(int requestingPlayerID)
    {
        players = StartGame.players;
        if (deck.Count >= cardsToDeal)
        {
            Debug.Log(StartGame.players.Count);
            foreach (var player in players)
                for (int i = 0; i < cardsToDeal; i++)
                {
                    if (PhotonNetwork.LocalPlayer.ActorNumber == player.id)
                    {
                        IssuingCards(player.id);
                        playerState.UpdateCard();
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

    private void IssuingCards(int id){
        // Выбираем случайную карту из колоды
        int randomIndex = Random.Range(0, deck.Count);

        photonView.RPC("DealingCard", RpcTarget.All, randomIndex, id);
        // Проверяем, какой игрок запросил карты
        soundManager.CardDealerSound();            
        // Если это локальный игрок, карты идут в его локальную руку
        StartCoroutine(MoveCardToHand(cardObject, playerHandPositions));
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
    [PunRPC]
    void DealingCard(int indexDelete, int id){
        Card selectedCard = deck[indexDelete];
        deck.RemoveAt(indexDelete);
        // Создаем объект карты
        cardObject = Instantiate(cardPrefab, cardParent.transform, false);
        cardObject.GetComponent<CardInfoScr>().ShowCardInfo(selectedCard);

        players[id - 1].cards.Add(selectedCard);
        if(id != PhotonNetwork.LocalPlayer.ActorNumber)
            Destroy(cardObject);
    }
    IEnumerator Timeout(){
        yield return new WaitForSeconds(animationDuration / 2);
    }

    
}

