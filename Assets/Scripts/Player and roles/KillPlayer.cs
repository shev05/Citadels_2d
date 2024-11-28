using Photon.Pun;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public GameObject KillPanel;
    PhotonView photonView;
    public static string roleNameKill;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    
    public void KillThief(){
        HidePanel();
        photonView.RPC("Kill", RpcTarget.All, "Thief");
    }
    public void KillMagician(){
        HidePanel();
        photonView.RPC("Kill", RpcTarget.All, "Magician");
    }
    public void KillKing(){
        HidePanel();
        photonView.RPC("Kill", RpcTarget.All, "King");    
    }
    public void KillBishop(){
        HidePanel();
        photonView.RPC("Kill", RpcTarget.All, "Bishop");  
    }
    public void KillMerchant(){
        HidePanel();
        photonView.RPC("Kill", RpcTarget.All, "Merchant");
    }
    public void KillArchitect(){
        HidePanel();
        photonView.RPC("Kill", RpcTarget.All, "Architect");
    }
    public void KillWarlord(){
        HidePanel();
        photonView.RPC("Kill", RpcTarget.All, "Warlord");
    }

    [PunRPC] 
    void Kill(string role){
        roleNameKill = role;
        foreach(var player in StartGame.players)
            if(player.role.Name == role)
                player.isKill = true;
        Debug.Log(role + " got killed");
    }
    void HidePanel(){
        KillPanel.SetActive(false);
        StartGame.players[PhotonNetwork.LocalPlayer.ActorNumber - 1].haveUlt = false;
    }
}
