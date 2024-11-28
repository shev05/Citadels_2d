using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ThiefPlayer : MonoBehaviour
{
    public GameObject ThiefPanel;
    PhotonView photonView;
    public List<Button> buttons;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    
    public void KillMagician(){
        HidePanel();
        photonView.RPC("Steal", RpcTarget.All, "Magician");
    }
    public void KillKing(){
        HidePanel();
        photonView.RPC("Steal", RpcTarget.All, "King");    
    }
    public void KillBishop(){
        HidePanel();
        photonView.RPC("Steal", RpcTarget.All, "Bishop");  
    }
    public void KillMerchant(){
        HidePanel();
        photonView.RPC("Steal", RpcTarget.All, "Merchant");
    }
    public void KillArchitect(){
        HidePanel();
        photonView.RPC("Steal", RpcTarget.All, "Architect");
    }
    public void KillWarlord(){
        HidePanel();
        photonView.RPC("Steal", RpcTarget.All, "Warlord");
    }

    [PunRPC]
    void Steal(string role){
        var player = StartGame.players[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        if(player.role.Name == role)
            player.robbed = true;
        Debug.Log(role + " got robbed");
    }
    void HidePanel(){
        
        ThiefPanel.SetActive(false);
        StartGame.players[PhotonNetwork.LocalPlayer.ActorNumber - 1].haveUlt = false;  
    }
}
