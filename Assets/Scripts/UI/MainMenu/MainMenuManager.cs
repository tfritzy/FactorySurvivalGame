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
        Settings,
        JoinVsCreateSelect,
        CreateMultiplayerGame,
        LobbyBrowser,
    }
    private Dictionary<Page, ActiveElement> pages = new();

    void Start()
    {
        pages = new()
        {
            { Page.MainMenu, new MainMenu() },
            { Page.Lobby, new Lobby() },
            { Page.Settings, new Settings() },
            { Page.JoinVsCreateSelect, new JoinVsCreate() },
            { Page.CreateMultiplayerGame, new CreateMultiplayerGame() },
            { Page.LobbyBrowser, new LobbyBrowser() },
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

        var joinVsCreateSelect = new JoinVsCreate();
        root.Add(joinVsCreateSelect);
        pages[Page.JoinVsCreateSelect] = joinVsCreateSelect;

        var createMultiplayerGame = new CreateMultiplayerGame();
        root.Add(createMultiplayerGame);
        pages[Page.CreateMultiplayerGame] = createMultiplayerGame;

        var lobbyBrowser = new LobbyBrowser();
        root.Add(lobbyBrowser);
        pages[Page.LobbyBrowser] = lobbyBrowser;

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