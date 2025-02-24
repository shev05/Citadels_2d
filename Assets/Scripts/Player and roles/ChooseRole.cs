using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class ChooseRole : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject cardField;
    [SerializeField] List<GameObject> removedCardSlots;
    
    static public List<RoleCard> roles;
    private List<RoleCard> removed = new List<RoleCard>();
    static public List<Player> players;
    private PhotonView photonView;
    
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        roles = new List<RoleCard>(RoleCardsManager.AllRoles);
    }

    void RemoveRoles(){
        removed = new List<RoleCard>();
        int randomIndexFirst = UnityEngine.Random.Range(0, roles.Count);
        removed.Add(roles[randomIndexFirst]);
        roles.RemoveAt(randomIndexFirst);
        int randomIndexSecond = UnityEngine.Random.Range(0, roles.Count);
        removed.Add(roles[randomIndexSecond]);
        roles.RemoveAt(randomIndexSecond);
        int randomIndexThird = UnityEngine.Random.Range(0, roles.Count);
        removed.Add(roles[randomIndexThird]);
        roles.RemoveAt(randomIndexThird);
        string[] removedCardNames = removed.Select(card => card.Name).ToArray();
        photonView.RPC("ActivateRemovedCardSlots", RpcTarget.All, removedCardNames);
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
        roles = new List<RoleCard>(RoleCardsManager.AllRoles);
        foreach (var player in players)
            if(player.isKing)
                indexPlayer = player.id;
        if (PhotonNetwork.LocalPlayer.ActorNumber == indexPlayer){
            ActivateChoosing();
        }
    }

    private void ActivateChoosing()
    {
        photonView.RPC("DeactivateRemovedCardSlots", RpcTarget.All);
        RemoveRoles();
        setupPanel();
        panel.SetActive(true);
    }

    [PunRPC]
    public void ActivateRemovedCardSlots(string[] removedCardNames)
    {
        for (int i = 0; i < removedCardSlots.Count && i < removedCardNames.Length; i++)
        {
            var card = RoleCardsManager.AllRoles.Find(r => r.Name == removedCardNames[i]);
            if (card != null)
            {
                if (i != 0)
                {
                    removedCardSlots[i].GetComponent<CardInfoScr>().ShowCardInfo(card);
                }
                removedCardSlots[i].SetActive(true);
            }
        }
    }
    
    [PunRPC]
    public void DeactivateRemovedCardSlots()
    {
        foreach(var slot in removedCardSlots) slot.SetActive(false);
    }
    
}
