using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnChoiseCard : MonoBehaviour
{
    GameObject selectedCard;

    private float liftAmount = 5f; // Сколько приподнять карту
    public Button selectCardButton; // Ссылка на кнопку выбора карты
    public Button buttonNextPlayer;
    public GameObject cardPrefab;
    public GameObject hand;
    public GameObject panel;
    public GameObject button;
    private PhotonView photonView;
    [SerializeField] TMP_Text[] cardCount; 


    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void SetSelectedCard(GameObject card)
    {
        if (selectedCard != null)
        {
            selectedCard.transform.position -= new Vector3(0, liftAmount, 0);
        }

        // Сохраняем новую карту и ее исходное положение
        selectedCard = card;

        // Приподнимаем карту
        selectedCard.transform.position += new Vector3(0, liftAmount, 0);

        // Показываем кнопку
        selectCardButton.gameObject.SetActive(true);
    }

    public void ChoiseButton_Click(){
        var chosenCard = selectedCard.GetComponent<CardInfoScr>().SelfCard;
        var index = CardDealer.deck.FindIndex(item => item.Name == chosenCard.Name);
        photonView.RPC("RemoveCard", RpcTarget.All, index, PhotonNetwork.LocalPlayer.ActorNumber);
        buttonNextPlayer.gameObject.SetActive(true);
        button.SetActive(false);
        panel.SetActive(false);
    }

    [PunRPC]
    private void RemoveCard(int index, int id){
        var player = StartGame.players[id - 1];
        var newCard = Instantiate(cardPrefab, hand.transform, false);
        newCard.GetComponent<CardInfoScr>().ShowCardInfo(CardDealer.deck[index]);
        player.cards.Add(newCard);
        cardCount[player.numberTable].text = player.cards.Count.ToString();
        CardDealer.deck.RemoveAt(index);
        GameTurnManager.activePlayer++;
        if(id != PhotonNetwork.LocalPlayer.ActorNumber)
            Destroy(newCard);
        else 
            player.isActive = true;

    }
}
