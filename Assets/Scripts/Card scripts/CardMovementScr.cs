using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovementScr : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera MainCamera;
    Vector3 offset;
    public Transform DefaultParent, DefaultTempCardParent;
    GameObject TempCardGO;
    private List<Player> _players;
    SoundManager soundManager;


    private void Awake()
    {
        MainCamera = Camera.allCameras[0];
        TempCardGO = GameObject.Find("TempCardGO");
        soundManager = FindObjectOfType<SoundManager>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        _players = StartGame.players;
        if (_players[PhotonNetwork.LocalPlayer.ActorNumber - 1].isActive == false) 
            TempCardGO.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.5f);
        else
        {
            TempCardGO.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        }
        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);

        DefaultParent = DefaultTempCardParent = transform.parent;

        TempCardGO.GetComponent<Image>().sprite = eventData.pointerDrag.GetComponent<CardInfoScr>().Logo.sprite;
        TempCardGO.transform.SetParent(DefaultParent);
        TempCardGO.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        soundManager.CardDealerSound();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = MainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + offset;

        if(TempCardGO.transform.parent != DefaultTempCardParent)
            TempCardGO.transform.SetParent(DefaultTempCardParent);

        CheckPos();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(DefaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(TempCardGO.transform.GetSiblingIndex());
        TempCardGO.transform.SetParent(GameObject.Find("Canvas").transform);
        TempCardGO.transform.localPosition = new Vector3(2240,0);
        if(DefaultParent.name.Equals("Hand 1 player")) this.enabled = false;
        soundManager.CardPutSound();
    }

    void CheckPos()
    {
        int newIndex = DefaultTempCardParent.childCount;

        for(int i = 0; i < DefaultTempCardParent.childCount; i++){
            if(transform.position.x < DefaultTempCardParent.GetChild(i).position.x)
            {
                newIndex = i;

                if(TempCardGO.transform.GetSiblingIndex() < newIndex) newIndex--;
                break;
            }
        }

        TempCardGO.transform.SetSiblingIndex(newIndex);
    }
}