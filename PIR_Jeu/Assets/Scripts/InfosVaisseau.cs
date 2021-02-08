using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfosVaisseau : MonoBehaviour
{
    public GameObject vies_ui;
    public GameObject score_ui;

    private Animator animator;

    public int points_de_vie = 3;

    public float temps_invincibilite = 1;
    private float invincible_time = 0;

    private float points_per_second = 10;
    private float score = 0;
    private bool stop_score = false;

    private float blink_period = 0.1f;
    private float blink_time = 0;

    private bool invincible = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!stop_score)
            score += Time.fixedDeltaTime * points_per_second;
        score_ui.GetComponent<Text>().text = "Score : " + Mathf.RoundToInt(score);
        if (invincible)
        {
            invincible_time += Time.fixedDeltaTime;
            blink_time += Time.fixedDeltaTime;
            if (blink_time > blink_period)
            {
                if (gameObject.GetComponent<Renderer>().enabled)
                    gameObject.GetComponent<Renderer>().enabled = false;
                else
                    gameObject.GetComponent<Renderer>().enabled = true;
                blink_time = 0;
            }
            if (invincible_time > temps_invincibilite)
            {
                invincible = false;
                gameObject.GetComponent<Renderer>().enabled = true;
                invincible_time = 0;
                blink_time = 0;
            }
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (!invincible)
        {
            points_de_vie -= damage;
            vies_ui.GetComponent<Text>().text = "Vies : " + Mathf.Max(0, points_de_vie);
            if (points_de_vie <= 0)
            {
                stop_score = true;
                animator.SetBool("dead", true);
            }
            else
                invincible = true;
        }
    }

    public void StopGame()
    {
        //Afficher l'UI de fin de partie
        Camera.main.GetComponent<ObstaclesSpawner>().enabled = false;
        gameObject.SetActive(false);
    }
}
