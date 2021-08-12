using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public void showMenu()
    {
        gameOverUI.SetActive(true);
    }

}
