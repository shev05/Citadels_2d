using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseRole : MonoBehaviour
{
    public GameObject panel;
    public GameObject cardPrefab;
    public GameObject cardField;
    static public List<RoleCard> roles;
    public List<RoleCard> removed = new List<RoleCard>();
    static public List<Player> players;
    private PhotonView photonView;




    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        roles = new List<RoleCard>(RoleCardsManager.AllRoles);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RemoveRoles(){
        int randomIndexFirst = UnityEngine.Random.Range(0, roles.Count);
        removed.Add(roles[randomIndexFirst]);
        roles.RemoveAt(randomIndexFirst);
        int randomIndexSecond = UnityEngine.Random.Range(0, roles.Count);
        removed.Add(roles[randomIndexSecond]);
        roles.RemoveAt(randomIndexSecond);
        int randomIndexThird = UnityEngine.Random.Range(0, roles.Count);
        removed.Add(roles[randomIndexThird]);
        roles.RemoveAt(randomIndexThird);
    }

    void setupPanel(){
        foreach (var card in roles){
            GameObject cardObject = Instantiate(cardPrefab, cardField.transform, false); // Создаем объект внутри канваса
            cardObject.GetComponent<CardInfoScr>().ShowCardInfo(card);
        }
    }

    public void startChoosing(){
        photonView.RPC("Choosing", RpcTarget.All);

    }

    [PunRPC]
    public void Choosing(){
        int indexPlayer = 0;
        players = StartGame.players;
        Debug.Log(StartGame.players.Count);
        foreach (var player in players)
            if(player.isKing)
                indexPlayer = player.id;
        if (PhotonNetwork.LocalPlayer.ActorNumber == indexPlayer){
            ActivateChoosing();
        }
    }

    public void ActivateChoosing(){
        RemoveRoles();
        setupPanel();
        panel.SetActive(true);
    }
}
