using UnityEngine.UIElements;

public class Lobby : Modal
{
    public Lobby()
    {
        Label title = new Label("Lobby");
        title.style.fontSize = 30;
        title.style.color = ColorTheme.Current.PrimaryText;
        modal.Add(title);

        MenuButton back = new MenuButton(
            "Back",
            () => { MainMenuManager.Instance.ShowPage(MainMenuManager.Page.MainMenu); }
        );
        modal.Add(back);
    }
}