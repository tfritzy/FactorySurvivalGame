using System.Linq;
using Core;
using HighlightPlus;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private Interactable? highlightedObject;

    void LateUpdate()
    {
        ListenForHotkeys();
        Interactable? i = RaycastHelper.GetInteractableUnderCursor();
        UpdateHighlight(i);

        if (i != null)
        {
            bool interacted = PerformInspections(i);
            if (interacted)
            {
                return;
            }
        }
    }

    private void UpdateHighlight(Interactable? i)
    {
        if (i != highlightedObject)
        {
            if (highlightedObject?.GameObject != null)
            {
                highlightedObject.GetHighlightEffect().highlighted = false;
            }

            highlightedObject = i;
            if (highlightedObject == null)
            {
                return;
            }
            else
            {
                i.GetHighlightEffect().highlighted = true;
            }
        }
    }

    public static bool InteractWithinSphere()
    {
        Vector3 origin = PlayerMono.Instance.transform.position;
        Collider[] hits = Physics.OverlapSphere(
            origin,
            2.25f,
            Layers.InteractableLayersMask);
        if (hits.Length > 0)
        {
            var ordered = hits.OrderBy(
                c => (c.transform.position - origin).sqrMagnitude)
                .ToList();

            if (ordered.Count < 2)
            {
                return false;
            }

            Interactable? i = RaycastHelper.FindInteractableInHierarchy(ordered[1].gameObject);
            if (i != null)
            {
                i.OnInteract();
                return true;
            }
        }

        return false;
    }

    private void ListenForHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    private bool PerformInspections(Interactable? i)
    {
        if (i == null)
        {
            return false;
        }

        if (ClickLog.GetLmbUp())
        {
            i.OnInspect();
            return true;
        }

        if (ClickLog.GetRmbUp())
        {
            i.OnInteract();
            return true;
        }

        return false;
    }

    private void Close()
    {
        UIManager.Instance.CloseCharacterInspector();
    }
}