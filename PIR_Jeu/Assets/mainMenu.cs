using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class mainMenu : MonoBehaviour
{
    public void startGamePlayer()
    {
        SceneManager.LoadScene("HumanPlayerScene");
    }
    public void startGameIA()
    {
        SceneManager.LoadScene("AIPlayerScene");
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
