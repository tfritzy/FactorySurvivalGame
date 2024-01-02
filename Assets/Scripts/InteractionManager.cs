using Core;
using HighlightPlus;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private Interactable? highlightedObject;

    void LateUpdate()
    {
        Interactable? i = RaycastHelper.GetInteractableUnderCursor();

        ListenForHotkeys();
        CheckOpenMenu(i);
        UpdateHighlight(i);
    }

    private void UpdateHighlight(Interactable? i)
    {
        if (i != highlightedObject)
        {
            if (highlightedObject != null)
            {
                highlightedObject.GetHighlightEffect().SetHighlighted(false);
            }

            if (i != null)
            {
                highlightedObject = null;
                return;
            }

            highlightedObject = i;
            if (i != null)
            {
                i.GetHighlightEffect().SetHighlighted(true);
            }
        }
    }

    private void ListenForHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    private void CheckOpenMenu(Interactable? i)
    {

        if (i == null)
        {
            return;
        }

        if (ClickLog.GetLmbUp())
        {
            i.OnInspect();
        }

        if (ClickLog.GetRmbUp())
        {
            i.OnInteract();
        }
    }

    private void Close()
    {
        UIManager.Instance.CloseCharacterInspector();
    }
}