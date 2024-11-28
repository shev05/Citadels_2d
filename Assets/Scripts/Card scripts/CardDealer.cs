using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CardDealer : MonoBehaviourPun
{
    public Transform playerHandPositions;
    public Transform cardParent;
    public int cardsToDeal = 4;
    public float animationDuration = 0.5f;
    public GameObject cardPrefab;
    public GameObject cardPref;
    List<Player> players;
    GameObject cardObject;
    public static List<Card> deck;
    public Canvas canvas;
    private PhotonView photonView;
    private UpdatePlayerState playerState;
    private SoundManager soundManager;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerState = FindObjectOfType<UpdatePlayerState>();
        soundManager = FindObjectOfType<SoundManager>();
        
        deck = new List<Card>(CardsManager.AllCards);
    }

    public void ClickStartButton(){
        Debug.Log("Game started");
        photonView.RPC("StartDealing", RpcTarget.All);
    }
    
    public void StartDealingWithCount(int count){
        for(int i = 0; i < count; i++){
            IssuingCards(PhotonNetwork.LocalPlayer.ActorNumber);
        }
        playerState.UpdateCard();
    }


    [PunRPC]
    public void StartDealing()
    {
        StartCoroutine(DealCards());
    }

    private IEnumerator DealCards()
    {
        players = StartGame.players;
        if (deck.Count >= cardsToDeal)
        {
            foreach (var player in players)
                for (int i = 0; i < cardsToDeal; i++)
                {
                    if (PhotonNetwork.LocalPlayer.ActorNumber == player.id)
                    {
                        IssuingCards(player.id);
                        playerState.UpdateCard();
                    }
                    
                    yield return new WaitForSeconds(animationDuration / 2);
                }
        }
        else
        {
            Debug.LogWarning("Не хватает карт в колоде для раздачи.");
        }
    }

    private void IssuingCards(int id){
        int randomIndex = Random.Range(0, deck.Count);
        
        photonView.RPC("DealingCard", RpcTarget.All, randomIndex, id);
        
        soundManager.CardDealerSound();            
        
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
        
        cardObject = Instantiate(cardPrefab, cardParent.transform, false);
        cardObject.GetComponent<CardInfoScr>().ShowCardInfo(selectedCard);

        players[id - 1].cards.Add(selectedCard);
        if(id != PhotonNetwork.LocalPlayer.ActorNumber)
            Destroy(cardObject);
        Debug.Log(players[id - 1].nickname + " got " + selectedCard.Name + " card");
    }
    IEnumerator Timeout(){
        yield return new WaitForSeconds(animationDuration / 2);
    }

    
}

