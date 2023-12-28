using Core;
using HighlightPlus;
using UnityEngine;

public class CharacterInspectionManager : MonoBehaviour
{
    private CharacterMono highlightedObject;

    void LateUpdate()
    {
        CharacterMono c = RaycastHelper.GetCharacterUnderCursor();

        ListenForHotkeys();
        CheckOpenMenu(c);
        UpdateHighlight(c);
    }

    private void UpdateHighlight(CharacterMono c)
    {
        if (c != highlightedObject)
        {
            if (highlightedObject != null)
            {
                highlightedObject.HighlightEffect.highlighted = false;
            }

            if (c != null && ((Character)c.Actual).IsPreview)
            {
                highlightedObject = null;
                return;
            }

            highlightedObject = c;
            if (c != null)
            {
                c.HighlightEffect.SetHighlighted(true);
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

    private void CheckOpenMenu(CharacterMono c)
    {
        if (ClickLog.GetMouseButtonUp())
        {
            if (c == null || ((Character)c.Actual).IsPreview)
            {
                return;
            }

            UIManager.Instance.OpenCharacterInspector((Character)c.Actual, Close);
        }
    }

    private void Close()
    {
        UIManager.Instance.CloseCharacterInspector();
    }
}