using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.UIElements;


public class ChooseRoleCardScr : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
   private NextPlayerRoleTurn cardSelectionManager;
    private float lastClickTime = 0f;
    private float doubleClickInterval = 0.5f;

    private void Start()
    {
        // Ищем `CardSelectionManager` в сценеош
        cardSelectionManager = FindObjectOfType<NextPlayerRoleTurn>();
    }

    /*private void OnMouseDown()
    {
        // Устанавливаем текущую карту как выделенную
        cardSelectionManager.SetSelectedCard(gameObject);
    }*/
    public void OnPointerClick(PointerEventData eventData)
    {
        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= doubleClickInterval)
                    {
                        Debug.Log("jnjnjnjn");
                        // Двойной клик
                        cardSelectionManager.OnSelectCardButtonClick(gameObject);
                    }
                    lastClickTime = Time.time;
    }        
    
}
