using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class UltScript : MonoBehaviour
{
    public GameObject KillPanel; // Ссылка на Panel всплывающего окна
    public GameObject ThiefPanel; // Ссылка на Panel всплывающего окна
    public List<GameObject> ButtonRole;
    public GameObject destructionPanel;

    private KeyCode activationKey = KeyCode.Q;

    // Start is called before the first frame update
    void Start()
    {
        KillPanel.SetActive(false); // Скрываем окно по умолчанию
    }
    
    void Update()
    {
        if (Input.GetKeyDown(activationKey))
            {
                Ult_Click(); // Показываем или скрываем окно
            }
    }
    
    public void Ult_Click()
    {
        int indexPlayer = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var players = StartGame.players;
        if(players[indexPlayer].isActive && players[indexPlayer].haveUlt){
            if(players[indexPlayer].role.Name == "Assassin")
                KillPanel.SetActive(!KillPanel.activeSelf);

            if(players[indexPlayer].role. Name == "Thief"){
                ThiefPanel.SetActive(!KillPanel.activeSelf);
                foreach (GameObject roleBut in ButtonRole)
                {
                    Debug.Log("Button"+KillPlayer.roleNameKill + "  lwkfj " + roleBut.name);
                    if(roleBut.name == "Button"+KillPlayer.roleNameKill){
                        roleBut.gameObject.SetActive(false);
                    }
                } 
            }
            if(players[indexPlayer].role.Name == "Warlord")
                destructionPanel.SetActive(!destructionPanel.activeSelf); // Не готово, нужно предусмотреть, что карта точно выбрана
        }
    }
}
