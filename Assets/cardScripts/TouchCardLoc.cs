using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCardLoc : MonoBehaviour
{
    private TurnChoiseCard cardSelectionManager;

    private void Start()
    {
        // Ищем `CardSelectionManager` в сцене
        cardSelectionManager = FindObjectOfType<TurnChoiseCard>();
    }

    private void OnMouseDown()
    {
        // Устанавливаем текущую карту как выделенную
        cardSelectionManager.SetSelectedCard(gameObject);
    }
}
