using UnityEngine.UIElements;

public class CreateMultiplayerGame : Modal
{
    public CreateMultiplayerGame()
    {
        Label title = new Label("Create game");
        title.style.fontSize = 30;
        title.style.color = ColorTheme.Current.PrimaryText;
        modal.Add(title);

        Button createGame = new Button();
        createGame.text = "Create";
        createGame.style.marginTop = 10;
        createGame.style.width = 200;
        createGame.style.height = 30;
        createGame.style.alignSelf = Align.Center;
        createGame.style.backgroundColor = ColorTheme.Current.PanelForegroundColor;
        createGame.style.color = ColorTheme.Current.PrimaryText;
        createGame.SetAllBorderRadius(5);
        createGame.clicked += async () =>
        {
            await ConnectionManager.Instance.StartHostConnection(
                () => { MainMenuManager.Instance.ShowPage(MainMenuManager.Page.Lobby); }
            );
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
            MainMenuManager.Instance.ShowPage(MainMenuManager.Page.JoinVsCreateSelect);
        };
    }
}