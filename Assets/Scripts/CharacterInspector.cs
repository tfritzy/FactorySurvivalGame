using Core;
using UnityEngine;

public class CharacterInspectionManager : MonoBehaviour
{
    private bool menuOpen = false;

    void Update()
    {
        ListenForHotkeys();
        CheckOpenMenu();
    }

    private void ListenForHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            close();
        }
    }

    private void CheckOpenMenu()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        CharacterMono c = RaycastHelper.GetCharacterUnderCursor();
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