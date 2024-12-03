using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class NextPlayerRoleTurn : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject cardField;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Button selectCardButton;
    
    List<GameObject> remainingCards;
    PhotonView photonView;
    GameTurnManager _gameTurnManager;
    List<Player> players;
    SoundManager soundManager;
    List<RoleCard> _roles;
    GameObject selectedCard;
    Vector3 originalPosition;
    float liftAmount = 5f;
    public List<GameObject> cardsOnField = new List<GameObject>();
    
    private void Start()
    {
        selectCardButton.gameObject.SetActive(false);

        photonView = GetComponent<PhotonView>();
        _gameTurnManager = FindObjectOfType<GameTurnManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }
    
    public void OnSelectCardButtonClick(GameObject cardRole)
    {
        soundManager.CardDealerSound();
        selectedCard = cardRole;
        cardsOnField = new List<GameObject>();
         // Инициализация карт на поле
        GameObject[] initialCards = GameObject.FindGameObjectsWithTag("Rolecard");
        foreach (var card in initialCards)
        {
            cardsOnField.Add(card);
        }

        // Изначально все карты на поле — это оставшиеся карты
        remainingCards = new List<GameObject>(cardsOnField);

        var roleCard = selectedCard.GetComponent<CardInfoScr>().SelfCard;
        if (roleCard != null)
        {
            // Удаляем карту из списка карт на поле
            cardsOnField.Remove(selectedCard);

            // Уничтожаем объект карты
            Destroy(selectedCard);

            // Обновляем список оставшихся карт
            remainingCards = new List<GameObject>(cardsOnField);

            // Сбрасываем выбор и скрываем кнопку
            selectedCard = null;
            selectCardButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("На объекте " + selectedCard.name + " нет компонента Renderer.");
        }
        int tableNumber = 0;
        
        foreach(var player in StartGame.players)
            if(player.id == PhotonNetwork.LocalPlayer.ActorNumber){
                tableNumber = player.id;
            }
        photonView.RPC("AddRolePlayer", RpcTarget.All, tableNumber, roleCard.Name);
        tableNumber = NextTurn(tableNumber);
        List<string> remainingCardNames = new List<string>();
        foreach (var card in remainingCards)
        {
            remainingCardNames.Add(card.GetComponent<CardInfoScr>().SelfCard.Name);
        }
        // Скрываем кнопку после выделения карты
        selectCardButton.gameObject.SetActive(false);
        panel.SetActive(false);
        string[] remainingCardNamesArray = remainingCardNames.ToArray();
        ClearHand();
        photonView.RPC("NextPlayerTurn", RpcTarget.AllBuffered, tableNumber, remainingCardNamesArray);
    }
    
    [PunRPC]
    void NextPlayerTurn(int tableNumber, string[] remainingCardNamesArray)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == tableNumber){
            foreach(var player in StartGame.players)
                if(player.isKing)
                    if(PhotonNetwork.LocalPlayer.ActorNumber == player.id){
                        photonView.RPC("NextStep", RpcTarget.All);
                        return;
                    }
            _roles = ChooseRole.roles;
            GameObject[] initialCards = GameObject.FindGameObjectsWithTag("Rolecard");
            foreach (var card in initialCards)
            {
                cardsOnField.Add(card);
            }


            List<GameObject> restoredRemainingCards = new List<GameObject>();
            List<string> remainingCardNames = new List<string>(remainingCardNamesArray);
            setupPanel(remainingCardNames);
            panel.SetActive(true);
        }
    }

    public void ActivateChoosing(List<GameObject> remainingCards){
        foreach (var card in remainingCards){
            card.transform.SetParent(cardField.transform, false);
        }
    }

    int NextTurn(int tableNumber){
        if(tableNumber == 4)
            tableNumber = 1;
        else
            tableNumber++;
        return tableNumber;
    }
    void setupPanel(List<string> remainingCardNames){
        Remove_roles(remainingCardNames);
        foreach (var card in _roles){
            GameObject cardObject = Instantiate(cardPrefab, cardField.transform, false); // Создаем объект внутри канваса
            cardObject.GetComponent<CardInfoScr>().ShowCardInfo(card);
        }
    }
    void Remove_roles(List<string> remainingCardNames){
        List<RoleCard> _rolesToRemove = new List<RoleCard>();

        foreach (var role in _roles)
        {
            if (!remainingCardNames.Contains(role.Name))
            {
                _rolesToRemove.Add(role);
            }
        }

        // Затем удаляем их из исходного списка
        foreach (var role in _rolesToRemove)
        {
            _roles.Remove(role);
        }
    }
    [PunRPC]
    void NextStep(){
        _gameTurnManager.NextTurn();
    }
    [PunRPC]
    void AddRolePlayer(int id, string name){
        players = StartGame.players;
        List<RoleCard> roles = ChooseRole.roles;
        foreach (var role in roles){
            if(role.Name == name)
                players[id - 1].role = role;
        }
        Debug.Log(players[id - 1].nickname + " chose " + name);
    }

    private void ClearHand()
    {
        foreach (Transform role in cardField.transform)
        {
            Destroy(role.gameObject);
        }
    }
}
