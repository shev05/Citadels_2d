using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.UIElements;


public class ChooseRoleCardScr : MonoBehaviour
{
    // Start is called before the first frame update
   private NextPlayerRoleTurn cardSelectionManager;

    private void Start()
    {
        // Ищем `CardSelectionManager` в сцене
        cardSelectionManager = FindObjectOfType<NextPlayerRoleTurn>();
    }

    private void OnMouseDown()
    {
        // Устанавливаем текущую карту как выделенную
        cardSelectionManager.SetSelectedCard(gameObject);
    }
            
    
}
