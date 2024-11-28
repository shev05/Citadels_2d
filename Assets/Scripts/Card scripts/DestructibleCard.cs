using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestructibleCard : MonoBehaviour, IPointerClickHandler
{
    private List<Player> _players;
    private DestructionManager _destructionManager;
    private float lastClickTime = 0f;
    private float doubleClickInterval = 0.5f;

    private void Start()
    {
        _destructionManager = FindObjectOfType<DestructionManager>();
    }
    
    private void Destruct()
    {
        _players = StartGame.players;
        var currentPlayer = _players[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        if (currentPlayer.role.Name.Equals("Warlord") && currentPlayer.isActive && currentPlayer.destructionAvaliable)
        {
            if (_destructionManager.FindPlayer(gameObject) is null || _destructionManager.FindPlayer(gameObject).role.Name.Equals("Bishop")
                                                         || gameObject.GetComponent<CardInfoScr>().SelfCard.Name.Equals("Keep"))
            {
                return;
            }
            _destructionManager.Destruction(gameObject);
            currentPlayer.destructionAvaliable = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        if (timeSinceLastClick <= doubleClickInterval)
        {
            Destruct();
        }
        lastClickTime = Time.time;
    }
}
