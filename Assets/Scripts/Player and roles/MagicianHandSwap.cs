using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MagicianHandSwap : MonoBehaviour
{
    [SerializeField] Transform hand;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<Button> avatarButtons;
    [SerializeField] GameObject dropPanel;
    
    private CardDealer cardDealer;
    private SoundManager soundManager;
    private List<Player> _players;
    private PhotonView _photonView;
    private GameObject _currentAvatar;
    private bool _playerSwapMode;
    private UpdatePlayerState playerState;
    
    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        playerState = FindObjectOfType<UpdatePlayerState>();
        cardDealer = FindObjectOfType<CardDealer>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Выполняем Raycast
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            // Проверяем, имеет ли объект нужный тег
            if (hit.collider.CompareTag("Avatar"))
            {
                // Проверяем, есть ли компонент SpriteRenderer и активная переменная
                var avatar = hit.collider.gameObject;

                if (avatar != null && _playerSwapMode)
                {
                    // Если навели на новый объект
                    if (_currentAvatar != avatar)
                    {
                        ResetCurrentAvatar(); // Сброс предыдущего аватара
                        _currentAvatar = avatar;
                        avatar.GetComponent<Image>().color = new Color(255, 0, 0, 255); // Меняем цвет
                    }
                }
            }
        }
        else
        {
            // Сбрасываем выделение, если курсор ни на что не указывает
            ResetCurrentAvatar();
        }
    }

    private void SwapPlayerCards(int tableNumber)
    {
        _players = StartGame.players;
        var swappedPlayer = _players.First(item => item.numberTable == tableNumber);
        _photonView.RPC("Swap", RpcTarget.All, swappedPlayer.id, PhotonNetwork.LocalPlayer.ActorNumber);
        _playerSwapMode = false;
        _players[PhotonNetwork.LocalPlayer.ActorNumber - 1].haveUlt = false;
        playerState.UpdateCard();
        foreach(var button in avatarButtons){
            button.interactable = false;
        }

    }

    [PunRPC]
    void Swap(int playerId, int localPlayerId)
    {
        _players = StartGame.players;
        var swappablePlayer = _players[playerId - 1];
        var localPlayer = _players[localPlayerId - 1];

        if (PhotonNetwork.LocalPlayer.ActorNumber == playerId ||
            PhotonNetwork.LocalPlayer.ActorNumber == localPlayerId)
        {
            foreach (Transform item in hand.transform)
            {
                Destroy(item.gameObject);
            }
        }

        List<Card> tempHand = new List<Card>(localPlayer.cards);
        localPlayer.cards = new List<Card>(swappablePlayer.cards);
        swappablePlayer.cards = new List<Card>(tempHand);
        if (PhotonNetwork.LocalPlayer.ActorNumber == playerId ||
            PhotonNetwork.LocalPlayer.ActorNumber == localPlayerId)
        {
            foreach (var item in _players[PhotonNetwork.LocalPlayer.ActorNumber - 1].cards)
            {
                var cardObject = Instantiate(cardPrefab, hand.transform, false);
                cardObject.GetComponent<CardInfoScr>().ShowCardInfo(item);
            }
        }

        soundManager.MagicianSwap();
        Debug.Log(localPlayer.nickname + " swapped his cards with " + swappablePlayer.nickname);
    }

    public void ActivatePlayerSwap(){
        foreach(var button in avatarButtons){
            button.interactable = true;
        }
        _playerSwapMode = true;
    }

    public void ActivateDeckSwap(){
        dropPanel.SetActive(true);
    }

    private void ResetCurrentAvatar()
    {
        if (_currentAvatar != null)
        {
            _currentAvatar.GetComponent<Image>().color = new Color(1, 1, 1, 0); // Возвращаем исходный цвет
            _currentAvatar = null;
        }
    }

    public void ClickPlayer2Button(){
        SwapPlayerCards(1);
    }

    public void ClickPlayer3Button(){
        SwapPlayerCards(2);
    }

    public void ClickPlayer4Button(){
        SwapPlayerCards(3);
    }

    public void ClickConfirmButton(){
        string[] droppedCards = new string[dropPanel.transform.GetChild(0).childCount];
        int i = 0;
        foreach(Transform item in dropPanel.transform.GetChild(0)){
            droppedCards[i++] = item.gameObject.GetComponent<CardInfoScr>().SelfCard.Name;
            Destroy(item.gameObject);
        }
        _photonView.RPC("DropCards", RpcTarget.All, droppedCards, PhotonNetwork.LocalPlayer.ActorNumber);
        cardDealer.StartDealingWithCount(droppedCards.Length);
        dropPanel.SetActive(false);
    }

    [PunRPC]
    void DropCards(string[] droppedCards, int playerId){
        _players = StartGame.players;
        foreach(var item in droppedCards){
            Card card = _players[playerId - 1].cards.First(card => card.Name.Equals(item));
            _players[playerId - 1].cards.Remove(card);
        }
    }
}
