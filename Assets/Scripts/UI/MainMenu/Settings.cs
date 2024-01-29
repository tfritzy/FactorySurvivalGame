using UnityEngine.UIElements;

public class Settings : Modal
{
    public Settings()
    {
        Label title = new Label("Settings");
        title.style.fontSize = 30;
        title.style.color = ColorTheme.Current.PrimaryText;
        modal.Add(title);

        MenuButton back = new MenuButton(
            "Back",
            () => MainMenuManager.Instance.ShowPage(MainMenuManager.Page.MainMenu)
        );
        modal.Add(back);
    }
}