using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public string levelToStart;
    public void RestartGame()
    {
        SceneManager.LoadScene(levelToStart);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
