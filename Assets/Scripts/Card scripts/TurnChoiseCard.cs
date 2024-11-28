using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TurnChoiseCard : MonoBehaviour
{
    GameObject selectedCard;

    private float liftAmount = 5f;
    public Button selectCardButton;
    public Button buttonNextPlayer;
    public GameObject cardPrefab;
    public GameObject hand;
    public GameObject panel;
    public GameObject button;
    private PhotonView photonView;
    private UpdatePlayerState playerState;
    SoundManager soundManager;
    
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerState = FindObjectOfType<UpdatePlayerState>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void SetSelectedCard(GameObject card)
    {
        if (selectedCard != null)
        {
            selectedCard.transform.position -= new Vector3(0, liftAmount, 0);
        }
        
        selectedCard = card;

        selectedCard.transform.position += new Vector3(0, liftAmount, 0);

        soundManager.CardDealerSound();

        selectCardButton.gameObject.SetActive(true);
    }

    public void ChoiseButton_Click(GameObject card){
        selectedCard = card;
        var chosenCard = selectedCard.GetComponent<CardInfoScr>().SelfCard;
        var index = CardDealer.deck.FindIndex(item => item.Name == chosenCard.Name);
        foreach (Transform item in panel.transform.GetChild(0)){
            Destroy(item.gameObject);
        }
        photonView.RPC("RemoveCard", RpcTarget.All, index, PhotonNetwork.LocalPlayer.ActorNumber);
        playerState.UpdateCard();
        buttonNextPlayer.gameObject.SetActive(true);
        button.SetActive(false);
        panel.SetActive(false);
    }

    [PunRPC]
    private void RemoveCard(int index, int id){
        var player = StartGame.players[id - 1];
        var newCard = Instantiate(cardPrefab, hand.transform, false);
        newCard.GetComponent<CardInfoScr>().ShowCardInfo(CardDealer.deck[index]);
        player.cards.Add(CardDealer.deck[index]);
        Debug.Log(player.nickname + " chose the " + CardDealer.deck[index].Name);
        CardDealer.deck.RemoveAt(index);
        GameTurnManager.activePlayer++;
        if(id != PhotonNetwork.LocalPlayer.ActorNumber)
            Destroy(newCard);
        else 
            player.isActive = true;

    }
}
