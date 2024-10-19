using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text textlastMessage;
    [SerializeField] TMP_InputField textMessageField;
    public CardDealer dealer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            photonView.RPC("DealCards", RpcTarget.All);
        }
    }

    private PhotonView photonView;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void SendButton()
    {
        photonView.RPC("Send_Data", RpcTarget.AllBuffered, PhotonNetwork.NickName,textMessageField.text);
    }

    [PunRPC]
    private void Send_Data(string nick, string message)
    {
        textlastMessage.text+= nick + ": " + message + "\n"; 
    }

    [PunRPC]
    public void DealCards()
    {
        StartCoroutine(dealer.DealCards());
    }
}