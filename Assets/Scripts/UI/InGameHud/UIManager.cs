using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;
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

        characterInspector = new CharacterInspector(() => { });
        root.Add(characterInspector);
    }

    void Update()
    {
        inGameHud?.Update();
        characterInspector?.Update();
    }

    public void OpenCharacterInspector(Character character)
    {
        characterInspector.SetCharacter(character);
        characterInspector.Show();
    }

    public void CloseCharacterInspector()
    {
        characterInspector?.Hide();
    }
}
