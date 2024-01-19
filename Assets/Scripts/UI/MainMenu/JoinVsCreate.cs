using UnityEngine.UIElements;

public class JoinVsCreate : Modal
{
    public JoinVsCreate()
    {
        Label title = new Label("Join or create game");
        title.style.fontSize = 30;
        title.style.color = ColorTheme.Current.PrimaryText;
        modal.Add(title);

        Button joinGame = new Button();
        joinGame.text = "Join game";
        joinGame.style.marginTop = 10;
        joinGame.style.width = 200;
        joinGame.style.height = 30;
        joinGame.style.alignSelf = Align.Center;
        joinGame.style.backgroundColor = ColorTheme.Current.PanelForegroundColor;
        joinGame.style.color = ColorTheme.Current.PrimaryText;
        joinGame.SetAllBorderRadius(5);
        joinGame.clicked += () =>
        {
            MainMenuManager.Instance.ShowPage(MainMenuManager.Page.LobbyBrowser);
        };
        modal.Add(joinGame);

        Button createGame = new Button();
        createGame.text = "Create game";
        createGame.style.marginTop = 10;
        createGame.style.width = 200;
        createGame.style.height = 30;
        createGame.style.alignSelf = Align.Center;
        createGame.style.backgroundColor = ColorTheme.Current.PanelForegroundColor;
        createGame.style.color = ColorTheme.Current.PrimaryText;
        createGame.SetAllBorderRadius(5);
        createGame.clicked += () =>
        {
            MainMenuManager.Instance.ShowPage(MainMenuManager.Page.CreateMultiplayerGame);
        };
        modal.Add(createGame);

        Button back = new Button();
        back.text = "Back";
        back.style.marginTop = 10;
        back.style.width = 100;
        back.style.height = 30;
        back.style.alignSelf = Align.Center;
        back.style.backgroundColor = ColorTheme.Current.PanelForegroundColor;
        back.style.color = ColorTheme.Current.PrimaryText;
        back.SetAllBorderRadius(5);
        modal.Add(back);

        back.clicked += () =>
        {
            MainMenuManager.Instance.ShowPage(MainMenuManager.Page.MainMenu);
        };
    }
}