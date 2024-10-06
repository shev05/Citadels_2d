using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameListItem : MonoBehaviour
{
    [SerializeField] Text textName;
    [SerializeField] Text textPlayerCount;

    public void SetInfo(RoomInfo info)
    {
        textName.text = info.Name;
        textPlayerCount.text = info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void JoinToListRoom(){
        PhotonNetwork.JoinRoom(textName.text);
    }

}
