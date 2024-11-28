using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
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
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Вы подключены к:" + PhotonNetwork.CloudRegion);
    
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
        Debug.LogError("Room creation failed");
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