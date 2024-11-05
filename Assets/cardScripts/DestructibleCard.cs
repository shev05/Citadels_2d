using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DestructibleCard : MonoBehaviour
{
    private List<Player> _players;
    private bool isClicked = false;
    private DestructionManager _destructionManager;

    private void Start()
    {
        _destructionManager = FindObjectOfType<DestructionManager>();
    }

    private void OnMouseDown()
    {
        _players = StartGame.players;
        var currentPlayer = _players[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        if (currentPlayer.role.Name.Equals("Warlord") && currentPlayer.isActive)
        {
            if (!isClicked)
            {
                isClicked = true;
                _destructionManager.clickedCard = gameObject;
                gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.8f);
            }
            else
            {
                _destructionManager.clickedCard = null;
                isClicked = false;
                gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f);
            }
        }
    }

    
}
