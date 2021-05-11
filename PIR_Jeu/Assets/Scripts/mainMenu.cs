using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class mainMenu : MonoBehaviour
{
    public static bool human_player = true;

    public void startGamePlayer()
    {
        human_player = true;
        SceneManager.LoadScene("HumanPlayerScene");

    }
    public void startGameIA()
    {
        human_player = false;
        SceneManager.LoadScene("MenuAI");
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
