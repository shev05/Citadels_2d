using System.Linq;
using Photon.Pun;
using UnityEngine;

public class PurpleActive : MonoBehaviour
{
    private CardDealer cardDealer;
    PhotonView photonView;
    UpdatePlayerState playerState;
    public GameObject cardPlace;
    public GameObject cardPanel;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        cardDealer = FindObjectOfType<CardDealer>();
        playerState = FindObjectOfType<UpdatePlayerState>();
    }

    public void ActiveCard(GameObject card){
        var player = StartGame.players[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        if(player.isActive){
            var chosenCard = (Card)card.GetComponent<CardInfoScr>().SelfCard;
            if(chosenCard.Name == "Smithy"){
                if(player.money >=2 && player.haveSmithyUlt){
                    photonView.RPC("SmithyUseCost", RpcTarget.All, player.id);
                    cardDealer.StartDealingWithCount(3);
                    playerState.UpdateMoney();
                    player.haveSmithyUlt = false;
                }
            }
            else if(chosenCard.Name == "Laboratory"){
                if(player.cards.Count >=1 && player.haveLaboratoryUlt){
                    cardPanel.SetActive(true);
                    player.haveLaboratoryUlt = false;
                }
            }
        }
    }
    [PunRPC]
    void SmithyUseCost(int id){
        var player = StartGame.players[id - 1];
        player.money -=2;
    }
    public void CardSale(){
        string droppedCards;
        var item = cardPlace.transform.GetChild(0);
        droppedCards = item.gameObject.GetComponent<CardInfoScr>().SelfCard.Name;
        Destroy(item.gameObject);     
        photonView.RPC("DropCards", RpcTarget.All, droppedCards, PhotonNetwork.LocalPlayer.ActorNumber);
        cardPanel.SetActive(false);
        photonView.RPC("LaboratoryAddMoney", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        playerState.UpdateMoney();
    }
    [PunRPC]
    void LaboratoryAddMoney(int id){
        var player = StartGame.players[id - 1];
        player.money +=1;
    }
    [PunRPC]
    void DropCards(string droppedCards, int playerId){
        var players = StartGame.players;
        Card card = players[playerId - 1].cards.First(card => card.Name.Equals(droppedCards));
        players[playerId - 1].cards.Remove(card);
    }
}
