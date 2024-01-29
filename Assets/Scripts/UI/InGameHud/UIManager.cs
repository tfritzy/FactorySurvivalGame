using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public InputAction pauseAction;

    private VisualElement root;
    private InGameHud inGameHud;
    private PauseMenu pauseMenu;
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

    public enum Page
    {
        InGameHud,
        PauseMenu,
    }

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        inGameHud = new InGameHud();
        root.Add(inGameHud);

        pauseMenu = new PauseMenu();
        root.Add(pauseMenu);

        characterInspector = new CharacterInspector(() => { });
        root.Add(characterInspector);
    }

    public void ShowPage(Page page)
    {
        switch (page)
        {
            case Page.InGameHud:
                inGameHud.Show();
                pauseMenu.Hide();
                break;
            case Page.PauseMenu:
                inGameHud.Hide();
                pauseMenu.Show();
                break;
        }
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

    public void OnPause()
    {
        GameStateActions.Pause();
    }
}
