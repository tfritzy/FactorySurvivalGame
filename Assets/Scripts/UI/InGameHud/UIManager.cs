using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private VisualElement root;
    private InGameHud inGameHud;
    public static ColorTheme ColorTheme;
    private CharacterInspector characterInspector;

    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }

            return _instance;
        }
    }

    void Start()
    {
        ColorTheme = new DarkTheme();

        root = GetComponent<UIDocument>().rootVisualElement;
        inGameHud = new InGameHud();
        root.Add(inGameHud);
    }

    void Update()
    {
        inGameHud.Update();
    }

    public void OpenCharacterInspector(Character character, System.Action onClose)
    {
        Debug.Log("Opening inspection menu");
        if (characterInspector == null)
        {
            characterInspector = new CharacterInspector(character, onClose);
            root.Add(characterInspector);
        }
        characterInspector.style.display = DisplayStyle.Flex;
    }

    public void CloseCharacterInspector()
    {
        characterInspector.style.display = DisplayStyle.None;
    }
}
