using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LobbyBrowser : Modal
{
    public LobbyBrowser()
    {
        Label title = new Label("Browse games");
        title.style.fontSize = 30;
        title.style.color = ColorTheme.Current.PrimaryText;
        modal.Add(title);

        MenuButton findRandomGame = new MenuButton(
            "Find random game",
            async () =>
            {
                await ConnectionManager.Instance.StartClientConnection();
            }
        );
        modal.Add(findRandomGame);

        MenuButton back = new MenuButton(
            "Back",
            () => { MainMenuManager.Instance.ShowPage(MainMenuManager.Page.MainMenu); }
        );
        modal.Add(back);
    }
}