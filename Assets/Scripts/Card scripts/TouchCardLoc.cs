using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCardLoc : MonoBehaviour, IPointerClickHandler
{
    private TurnChoiseCard cardSelectionManager;
    private float lastClickTime = 0f;
    private float doubleClickInterval = 0.5f;

    private void Start()
    {
        cardSelectionManager = FindObjectOfType<TurnChoiseCard>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        if (timeSinceLastClick <= doubleClickInterval)
        {
            cardSelectionManager.ChoiseButton_Click(gameObject);
        }
        lastClickTime = Time.time;
    }
}
