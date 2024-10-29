using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTurnManager : MonoBehaviour
{
    private List<Player> _players;
    private List<Player> turnBasedPlayerList;
    public List<TMP_Text> roleTexts;
    private PhotonView photonView;
    public Button nextTurnButton;
    private int tableNumber;

    private int activePlayer = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTurn()
    {
        _players = StartGame.players;
        turnBasedPlayerList = _players.OrderBy(turn => turn.role.TurnNum).ToList();

        TurnStep();
    }

    [PunRPC]
    void showRole(string role){
        roleTexts[tableNumber].text = role;
    }

    public void ButtonNextTurn_Click(){
        photonView.RPC("TurnStep", RpcTarget.All);
        nextTurnButton.gameObject.SetActive(false);
    }

    [PunRPC]
    private void TurnStep(){
        if (activePlayer >= turnBasedPlayerList.Count) return;
        var player = turnBasedPlayerList[activePlayer++];
        tableNumber = player.numberTable;
            if(player.id == PhotonNetwork.LocalPlayer.ActorNumber){
                nextTurnButton.gameObject.SetActive(true);
                photonView.RPC("showRole", RpcTarget.All, player.role.Name);
            }
    }
}
