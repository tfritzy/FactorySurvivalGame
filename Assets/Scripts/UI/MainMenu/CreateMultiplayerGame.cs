using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CreateMultiplayerGame : Modal
{
    public CreateMultiplayerGame()
    {
        Label title = new Label("Create game");
        title.style.fontSize = 30;
        title.style.color = ColorTheme.Current.PrimaryText;
        modal.Add(title);

        MenuButton createGame = new MenuButton(
            "Create",
            async () =>
            {
                await ConnectionManager.Instance.StartHostConnection();
            }
        );
        modal.Add(createGame);

        MenuButton back = new MenuButton(
            "Back",
            () => { MainMenuManager.Instance.ShowPage(MainMenuManager.Page.JoinVsCreateSelect); }
        );
        modal.Add(back);
    }
}