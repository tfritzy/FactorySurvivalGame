using UnityEngine.UIElements;

public class Settings : Modal
{
    public Settings()
    {
        Label title = new Label("Settings");
        title.style.fontSize = 30;
        title.style.color = ColorTheme.Current.PrimaryText;
        modal.Add(title);

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