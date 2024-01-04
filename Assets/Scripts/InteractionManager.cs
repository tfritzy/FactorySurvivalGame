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

        if (Input.GetKey(KeyCode.Space))
        {
            bool interacted = InteractWithinSphere();
            if (interacted)
            {
                return;
            }
        }

        if (Input.GetMouseButton(1))
        {
            RightClickMove();
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

    private bool InteractWithinSphere()
    {
        Vector3 origin = PlayerMono.Instance.transform.position;
        Collider[] hits = Physics.OverlapSphere(
            origin,
            2.25f,
            Layers.InteractableLayersMask);

        if (hits.Length > 0)
        {
            var closest = hits.OrderBy(
                c => (c.transform.position - origin).sqrMagnitude).First();
            Interactable? i = RaycastHelper.FindInteractableInHierarchy(closest.gameObject);
            if (i != null)
            {
                i.OnInteract();
                return true;
            }
        }

        return false;
    }

    private void RightClickMove()
    {
        Vector3? point = RaycastHelper.GetGroundPointUnderCursor();
        if (point != null)
        {
            PlayerMono.Instance.MoveCommand(point.Value);
        }
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