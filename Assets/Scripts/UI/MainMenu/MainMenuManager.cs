using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    public Page CurrentPage { get; private set; } = Page.MainMenu;
    private VisualElement? root;

    private static MainMenuManager? _instance;
    public static MainMenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<MainMenuManager>();
            }

            return _instance;
        }
    }

    public enum Page
    {
        MainMenu,
        Lobby,
        Settings
    }
    private Dictionary<Page, ActiveElement> pages = new();

    void Start()
    {
        pages = new()
        {
            { Page.MainMenu, new MainMenu() },
            { Page.Lobby, new Lobby() },
            { Page.Settings, new Settings() }
        };

        root = GetComponent<UIDocument>().rootVisualElement;

        var mainMenu = new MainMenu();
        root.Add(mainMenu);
        pages[Page.MainMenu] = mainMenu;

        var lobby = new Lobby();
        root.Add(lobby);
        pages[Page.Lobby] = lobby;

        var settings = new Settings();
        root.Add(settings);
        pages[Page.Settings] = settings;

        ShowPage(Page.MainMenu);
    }


    public void ShowPage(Page page)
    {
        CurrentPage = page;
        foreach (var pageElement in pages)
        {
            if (pageElement.Key == page)
            {
                pageElement.Value.Show();
            }
            else
            {
                pageElement.Value.Hide();
            }
        }
    }
}