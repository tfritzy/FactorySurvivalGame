using UnityEngine.UIElements;

public class JoinVsCreate : Modal
{
    public JoinVsCreate()
    {
        Label title = new Label("Join or create game");
        title.style.fontSize = 30;
        title.style.color = ColorTheme.Current.PrimaryText;
        modal.Add(title);

        MenuButton joinGame = new MenuButton(
            "Join game",
            () => { MainMenuManager.Instance.ShowPage(MainMenuManager.Page.LobbyBrowser); }
        );
        modal.Add(joinGame);

        MenuButton createGame = new MenuButton(
            "Create game",
            () => { MainMenuManager.Instance.ShowPage(MainMenuManager.Page.CreateMultiplayerGame); }
        );
        modal.Add(createGame);

        MenuButton back = new MenuButton(
            "Back",
            () => { MainMenuManager.Instance.ShowPage(MainMenuManager.Page.MainMenu); }
        );
        modal.Add(back);
    }
}