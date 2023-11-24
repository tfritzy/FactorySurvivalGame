using Core;
using HighlightPlus;
using UnityEngine;

public class CharacterInspectionManager : MonoBehaviour
{
    private bool menuOpen = false;
    public HighlightProfile HighlightProfile;
    private CharacterMono highlightedObject;

    void Update()
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
                Destroy(highlightedObject?.gameObject.GetComponent<HighlightEffect>());
            }

            highlightedObject = c;

            if (c != null)
            {
                var he = highlightedObject?.gameObject.AddComponent<HighlightEffect>();
                he.ProfileLoad(HighlightProfile);
                he.highlighted = true;
            }
        }
    }

    private void ListenForHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            close();
        }
    }

    private void CheckOpenMenu(CharacterMono c)
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (c == null)
        {
            return;
        }

        menuOpen = true;
        UIManager.Instance.OpenCharacterInspector((Character)c.Actual, close);
    }

    private void close()
    {
        UIManager.Instance.CloseCharacterInspector();
        menuOpen = false;
    }
}