using UnityEngine;
using UnityEngine.EventSystems;


public class ChooseRoleCardScr : MonoBehaviour, IPointerClickHandler
{
    private NextPlayerRoleTurn cardSelectionManager;
    private float lastClickTime = 0f;
    private float doubleClickInterval = 0.5f;

    private void Start()
    {
        cardSelectionManager = FindObjectOfType<NextPlayerRoleTurn>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        if (timeSinceLastClick <= doubleClickInterval)
        {
            cardSelectionManager.OnSelectCardButtonClick(gameObject);
        }
        lastClickTime = Time.time;
    }

}
