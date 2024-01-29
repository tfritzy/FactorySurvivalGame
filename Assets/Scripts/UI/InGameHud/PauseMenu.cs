using UnityEngine.UIElements;

public class PauseMenu : Modal
{
    public PauseMenu()
    {
        var pausedLabel = new Label("Paused");
        pausedLabel.style.fontSize = 30;
        pausedLabel.style.color = ColorTheme.Current.PrimaryText;
        modal.Add(pausedLabel);

        var resumeButton = new MenuButton(
            "Resume",
            () => GameStateActions.Unpause()
        );
        modal.Add(resumeButton);

        var settingsButton = new MenuButton(
            "Settings",
            () => UnityEngine.Debug.Log("Settings")
        );
        modal.Add(settingsButton);

        var returnToMainMenuButton = new MenuButton("\"Save\" and exit", GameStateActions.ReturnToMainMenu);
        modal.Add(returnToMainMenuButton);
    }
}