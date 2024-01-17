public class LobbyList : Modal
{
    public LobbyList()
    {
        Label title = new Label("Open games");
        title.style.fontSize = 30;
        title.style.color = ColorTheme.Current.PrimaryText;
        modal.Add(title);

        Button findRandomGame = new Button();
        findRandomGame.text = "Find random game";
        findRandomGame.style.marginTop = 10;
        findRandomGame.style.width = 200;
        findRandomGame.style.height = 30;
        findRandomGame.style.alignSelf = Align.Center;
        findRandomGame.style.backgroundColor = ColorTheme.Current.PanelForegroundColor;
        findRandomGame.style.color = ColorTheme.Current.PrimaryText;
        findRandomGame.SetAllBorderRadius(5);
        findRandomGame.clicked += () =>
        {
            MainMenuManager.Instance.ShowPage(MainMenuManager.Page.Lobby);
        };

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