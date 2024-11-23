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
            _destructionManager.clickedCard = gameObject;
            if (_destructionManager.FindPlayer() is null || _destructionManager.FindPlayer().role.Name.Equals("Bishop")
                                                         || gameObject.GetComponent<CardInfoScr>().SelfCard.Name.Equals("Keep"))
            {
                _destructionManager.clickedCard = null;
                return;
            }
            if (_destructionManager.prevCard is null)
            {
                _destructionManager.prevCard = gameObject;
                _destructionManager.destroyButton.SetActive(true);
                gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.8f);
            }
            else if (_destructionManager.clickedCard.Equals(_destructionManager.prevCard))
            {
                _destructionManager.clickedCard = null;
                _destructionManager.prevCard = null;
                _destructionManager.destroyButton.SetActive(false);
                gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f);
            }
            else if (!_destructionManager.clickedCard.Equals(_destructionManager.prevCard))
            {
                _destructionManager.prevCard.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f);
                _destructionManager.prevCard = gameObject;
                gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.8f);
            }
        }
    }

    
}
