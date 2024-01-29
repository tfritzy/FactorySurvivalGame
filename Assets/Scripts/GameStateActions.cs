using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameStateActions
{
    public static void Pause()
    {
        UIManager.Instance.ShowPage(UIManager.Page.PauseMenu);
        Time.timeScale = 0;
    }

    public static void Unpause()
    {
        UIManager.Instance.ShowPage(UIManager.Page.InGameHud);
        Time.timeScale = 1;
    }

    public static void ReturnToMainMenu()
    {
        ConnectionManager.Instance?.ResetState();
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}