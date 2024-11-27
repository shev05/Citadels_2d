using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] string region;
    [SerializeField] TMP_InputField nickName;

    [SerializeField] TMP_InputField  RoomName;
    [SerializeField] GameListItem itemPrefab;
    [SerializeField] Transform content;
    List<RoomInfo> allRoomsInfo = new List<RoomInfo>();

    private GameObject player;
    [SerializeField] GameObject player_pref;
    void Start()
    {
        
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
        Camera mainCamera = Camera.main;

        /*if(SceneManager.GetActiveScene().name == "First")
        {
            /*if(PhotonNetwork.CurrentRoom.PlayerCount == 1){
                float screenHeight = mainCamera.orthographicSize * 2; 
                Vector3 spawnPosition = new Vector3(0, -screenHeight / 2 + 0.5f, 0); 
                player = PhotonNetwork.Instantiate(player_pref.name, spawnPosition, Quaternion.identity);
                Sprite newSprite = Resources.Load<Sprite>("picture/Player/player1");
                player.GetComponent<SpriteRenderer>().sprite = newSprite;
            }
            if(PhotonNetwork.CurrentRoom.PlayerCount == 2){
                float screenHeight = mainCamera.orthographicSize * 2; 
                Vector3 spawnPosition = new Vector3(0, screenHeight / 2 - 0.5f, 0);
                player = PhotonNetwork.Instantiate(player_pref.name, spawnPosition, Quaternion.identity);
                Sprite newSprite = Resources.Load<Sprite>("picture/Player/player2");
                player.GetComponent<SpriteRenderer>().sprite = newSprite;
            }
            if(PhotonNetwork.CurrentRoom.PlayerCount == 3){
                float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;
                Vector3 spawnPosition = new Vector3(-screenWidth + 0.5f, 0, 0);
                player = PhotonNetwork.Instantiate(player_pref.name, spawnPosition, Quaternion.identity);
                Sprite newSprite = Resources.Load<Sprite>("picture/Player/player3");
                player.GetComponent<SpriteRenderer>().sprite = newSprite;
            }
            
        }*/
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Вы подключены к:" + PhotonNetwork.CloudRegion);
        Debug.Log(nickName.text);
    
        if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Вы отключены от сервара!");
    }

    public void CreateRoomBUtton()
    {
        if(!PhotonNetwork.IsConnected)
            return;

        if (!string.IsNullOrEmpty(nickName.text))
        {
            Hashtable playerProperties = new Hashtable
            {
                { "NickName", nickName.text }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Создана комната :" + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Плаки плаки?)");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Очистить старый список комнат
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        allRoomsInfo.Clear();

        foreach (RoomInfo info in roomList)
        {
            // Убедитесь, что комната доступна
            if (info.RemovedFromList || info.PlayerCount == 0)
                continue;

            // Добавить информацию о комнате в список
            GameListItem listItem = Instantiate(itemPrefab, content);
            if (listItem != null)
            {
                listItem.SetInfo(info);
                allRoomsInfo.Add(info);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        if (!string.IsNullOrEmpty(nickName.text))
        {
            Hashtable playerProperties = new Hashtable
            {
                { "NickName", nickName.text }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }
        PhotonNetwork.LoadLevel("First");
    }

    public void JoinRandRoomButton()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinButton()
    {
       

        StartCoroutine(WaitAndJoinRoom());
    }

    public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Destroy(player.gameObject);
        PhotonNetwork.LoadLevel("Menu");
    }
    
    private IEnumerator WaitAndJoinRoom()
    {
        yield return new WaitForSeconds(0.5f); // Дайте время для синхронизации
       
        PhotonNetwork.JoinRoom(RoomName.text);
    }

}