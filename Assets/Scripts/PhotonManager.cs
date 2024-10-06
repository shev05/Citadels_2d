using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

        if(SceneManager.GetActiveScene().name == "First")
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1){
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
            if(PhotonNetwork.CurrentRoom.PlayerCount == 4){
                float screenWidth = mainCamera.orthographicSize * mainCamera.aspect * 2; 
                Vector3 spawnPosition = new Vector3(screenWidth / 2 - 0.5f, 0, 0); 
                player = PhotonNetwork.Instantiate(player_pref.name, spawnPosition, Quaternion.identity);
                Sprite newSprite = Resources.Load<Sprite>("picture/Player/player4");
                player.GetComponent<SpriteRenderer>().sprite = newSprite;
            }
        }
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

        if(nickName.text == "")
            PhotonNetwork.NickName ="User";
        else
            PhotonNetwork.NickName = nickName.text;

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
        foreach (RoomInfo info in roomList)
        {
            for(int i = 0; i < allRoomsInfo.Count; i++){
                if(allRoomsInfo[i].masterClientId == info.masterClientId)
                    return;
            }

            GameListItem listItem = Instantiate(itemPrefab, content);
            if (listItem != null){
               listItem.SetInfo(info);
                allRoomsInfo.Add(info);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("First");
    }

    public void JoinRandRoomButton()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinButton()
    {
        PhotonNetwork.JoinRoom(RoomName.text);
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

}
