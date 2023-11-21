using Core;
using UnityEngine;

public class CharacterInspectionManager : MonoBehaviour
{
    private bool menuOpen = false;

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (menuOpen)
        {
            return;
        }

        CharacterMono c = RaycastHelper.GetCharacterUnderCursor();
        Debug.Log("Found character " + c?.gameObject?.name);
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