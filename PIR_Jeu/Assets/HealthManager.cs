using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public AudioSource hit_audio;
    public AudioSource explosion_audio;

    //Références vers les zones de texte où l'on doit afficher le score et le nombre de vies restantes
    public GameObject health_ui;
    public GameObject shield;
    public GameObject obstacle_spawner;

    //Référence vers le gestionnaire des animations du vaisseau
    private Animator animator;

    //Nombre de vies restantes avant d'enclencher la fin du jeu
    public int points_de_vie = 3;

    //Infos pour le calcul de l'invincibilité (modifiable pour ajuster la difficulté)
    private bool invincible = false;
    public float temps_invincibilite = 1;
    private float invincible_time = 0;
    //Détermine la vitesse de clignotement
    private float blink_period = 0.1f;
    private float blink_time = 0;

    public ScoreManager score_manager;

    public GameObject ship;

    private ShipSpawner ship_spawner;

    private void Start()
    {
        hit_audio = GameObject.FindWithTag("Sound").transform.GetChild(1).GetComponent<AudioSource>();
        explosion_audio = GameObject.FindWithTag("Sound").transform.GetChild(2).GetComponent<AudioSource>();

        animator = ship.GetComponent<Animator>();
    }

    public void SetShipSpawner(ShipSpawner ship_spawner)
    {
        this.ship_spawner = ship_spawner;
    }

    private void FixedUpdate()
    {
        //On détermine si le vaisseau est invincible ou non
        if (invincible)
        {
            invincible_time += Time.fixedDeltaTime;
            blink_time += Time.fixedDeltaTime;
            if (blink_time > blink_period)
            {
                if (ship.GetComponent<Renderer>().enabled)
                    ship.GetComponent<Renderer>().enabled = false;
                else
                    ship.GetComponent<Renderer>().enabled = true;
                blink_time = 0;
            }
            if (invincible_time > temps_invincibilite)
            {
                invincible = false;
                ship.GetComponent<Renderer>().enabled = true;
                invincible_time = 0;
                blink_time = 0;
            }
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (!shield.activeSelf)
        {
            if (!invincible)
            {
                points_de_vie -= damage;
                if (health_ui != null)
                    health_ui.GetComponent<TextMeshProUGUI>().text = "Vies : " + Mathf.Max(0, points_de_vie);
                if (points_de_vie == 0)
                {
                    explosion_audio.Play(0);
                    animator.SetBool("dead", true);
                    ship_spawner.HandleShipDeath(transform.parent.gameObject);
                }
                else
                {
                    hit_audio.Play(0);
                    invincible = true;
                }
            }
        }

    }

    /*public void StopGame()
    {
        score_manager.StopScore();
        //Afficher l'UI de fin de partie
        //obstacle_spawner.GetComponent<ObstaclesSpawner>().enabled = false;
        ship.SetActive(false);
    }*/
}
