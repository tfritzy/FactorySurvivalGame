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

        MenuButton singlePlayer = new MenuButton(
            "Single Player",
            () => MainMenuManager.Instance.ShowPage(MainMenuManager.Page.Lobby)
        );
        modal.Add(singlePlayer);

        MenuButton multiplayer = new MenuButton(
            "Multiplayer",
            () => MainMenuManager.Instance.ShowPage(MainMenuManager.Page.JoinVsCreateSelect)
        );
        modal.Add(multiplayer);

        MenuButton settings = new MenuButton(
            "Settings",
            () => MainMenuManager.Instance.ShowPage(MainMenuManager.Page.Settings)
        );
        modal.Add(settings);

        MenuButton quit = new MenuButton(
            "Quit",
            () => Application.Quit()
        );
        modal.Add(quit);
    }

    private void StyleMainMenuButton(MenuButton button)
    {
        button.style.width = 200;
        button.style.height = 30;
        button.style.alignSelf = Align.Center;
        button.style.backgroundColor = ColorTheme.Current.PanelForegroundColor;
        button.style.color = ColorTheme.Current.PrimaryText;
        button.SetAllBorderRadius(5);
    }
}