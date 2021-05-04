using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public bool show_score = true;

    public DifficultyManager difficulty_manager;

    private GameObject score_ui;

    //Infos pour le calcul du score
    private float points_per_second = 10;
    private float score = 0;
    private bool stop_score = false;

    private void Start()
    {
        score_ui = transform.GetChild(0).transform.GetChild(0).gameObject;

        score_ui.SetActive(show_score);
    }

    public void StopScore()
    {
        stop_score = true;
    }

    public void ResetScore()
    {
        score = 0;
        score_ui.GetComponent<TextMeshProUGUI>().text = "Score : " + 0;

        difficulty_manager.ResetDifficulty();
    }

    private void FixedUpdate()
    {
        //On calcule et on affiche le score
        if (!stop_score)
        {
            score += Time.fixedDeltaTime * points_per_second;
            difficulty_manager.SetScore(score);
        }
        score_ui.GetComponent<TextMeshProUGUI>().text = "Score : " + Mathf.RoundToInt(score);
    }
}
