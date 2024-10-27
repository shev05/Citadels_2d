using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;


public class ChooseRoleCardScr : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    private float lastClickTime;
    private const float doubleClickThreshold = 0.3f; // Время между кликами для двойного клика
    static public List<Player> players;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            // Двойной клик
            SelectCard();
        }

        lastClickTime = Time.time;
    }

    private void SelectCard()
    {

        Debug.Log("Карта выбрана двойным кликом: " + gameObject.GetComponent<CardInfoScr>().SelfCard.Name);
        var card = gameObject.GetComponent<CardInfoScr>().SelfCard;
        players = new List<Player>();
                foreach (var player in PhotonNetwork.PlayerList){
                    if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                        players[player.ActorNumber].role = card;
                }
        
    }
}
