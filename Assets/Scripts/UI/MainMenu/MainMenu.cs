using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : Modal
{
    public MainMenu()
    {
        Label title = new Label("Main Menu");
        title.style.fontSize = 30;
        title.style.color = ColorTheme.Current.PrimaryText;
        modal.Add(title);

        Button singlePlayer = new Button();
        singlePlayer.text = "Single Player";
        singlePlayer.style.marginTop = 10;
        StyleMainMenuButton(singlePlayer);
        singlePlayer.clicked += () =>
        {
            MainMenuManager.Instance.ShowPage(MainMenuManager.Page.Lobby);
        };
        modal.Add(singlePlayer);

        Button multiplayer = new Button();
        multiplayer.text = "Multiplayer";
        multiplayer.style.marginTop = 10;
        StyleMainMenuButton(multiplayer);
        multiplayer.clicked += () =>
        {
            MainMenuManager.Instance.ShowPage(MainMenuManager.Page.JoinVsCreateSelect);
        };
        modal.Add(multiplayer);

        Button settings = new Button();
        settings.text = "Settings";
        settings.style.marginTop = 10;
        StyleMainMenuButton(settings);
        settings.clicked += () =>
        {
            MainMenuManager.Instance.ShowPage(MainMenuManager.Page.Settings);
        };
        modal.Add(settings);

        Button quit = new Button();
        quit.text = "Quit";
        quit.style.marginTop = 10;
        StyleMainMenuButton(quit);
        quit.clicked += () =>
        {
            UnityEngine.Debug.Log("Quit");
            Application.Quit();
        };
        modal.Add(quit);
    }

    private void StyleMainMenuButton(Button button)
    {
        button.style.width = 200;
        button.style.height = 30;
        button.style.alignSelf = Align.Center;
        button.style.backgroundColor = ColorTheme.Current.PanelForegroundColor;
        button.style.color = ColorTheme.Current.PrimaryText;
        button.SetAllBorderRadius(5);
    }
}