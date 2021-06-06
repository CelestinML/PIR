using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    private List<float> scores;

    public int nb_ships = 1;

    public bool allow_weapons = true;

    public bool human_player;
    public GameOverManager game_over_manager;

    public ScoreManager score_manager;
    public ObstaclesSpawner obstacle_spawner;

    public GameObject ship_prefab;

    public GameObject piercing_shot_ui, piercing_image, cooldown_sprite, cooldown_text, health_ui;

    private List<GameObject> ships;
    private int nb_alive_ships;

    public Transform projectile_barrier;

    public Transform column_positions_transform;
    private List<Vector3> column_positions;

    public GameObject agent;

    public Transform border_left, border_right;

    private void Start()
    {
        scores = new List<float>();

        if (!allow_weapons)
        {
            piercing_shot_ui.SetActive(false);
            piercing_image.SetActive(false);
            cooldown_sprite.SetActive(false);
            cooldown_text.SetActive(false);
        }

        if (nb_ships > 1)
            health_ui.SetActive(false);

        ships = new List<GameObject>();

        SpawnShips();
    }

    private void SpawnShips()
    {
        nb_alive_ships = nb_ships;
        for (int i = 0; i < nb_ships; i++)
        {
            ships.Add(Instantiate(ship_prefab, transform));
            ships[ships.Count - 1].GetComponentInChildren<HealthManager>().SetShipSpawner(this);
            ships[ships.Count - 1].GetComponentInChildren<WeaponsManager>().allow_weapons = allow_weapons;
            ships[ships.Count - 1].GetComponentInChildren<WeaponsManager>().projectile_barrier = projectile_barrier;
            ships[ships.Count - 1].GetComponentInChildren<Movement>().column_positions = column_positions;
            if (!human_player)
                Instantiate(agent, ships[ships.Count - 1].transform);

        }
        if (nb_ships == 1)
        {
            ships[0].GetComponentInChildren<HealthManager>().health_ui = health_ui;
            ships[0].GetComponentInChildren<WeaponsManager>().piercing_shot_ui = piercing_shot_ui;
            ships[0].GetComponentInChildren<WeaponsManager>().piercing_image = piercing_image;
            ships[0].GetComponentInChildren<WeaponsManager>().cooldown_sprite = cooldown_sprite;
        }
    }

    public int GetIndex(GameObject ship)
    {
        return ships.IndexOf(ship);
    }

    public void HandleShipDeath(GameObject ship)
    {
        nb_alive_ships--;
        if (human_player)
        {
            game_over_manager.showMenu();
        }
        else
        {
            EnvironmentSpawner.scores.Add(score_manager.score);

            //alive_ships.Remove(ship);

            if (nb_alive_ships == 0)
            {
                Debug.Log("Reloading game");
                ReloadGame();
            }
        }
    }

    public void ReloadGame()
    {
        EnvironmentSpawner.scores.AddRange(scores);
        scores.Clear();

        ships = new List<GameObject>();

        if (nb_ships == 1)
        {
            health_ui.GetComponent<TextMeshProUGUI>().text = "Health Points : 3";
        }

        //Pause and reset the score
        score_manager.ResetScore();
        score_manager.enabled = false;

        //Pause the obstacle spawner and destroy the asteroids
        obstacle_spawner.enabled = false;
        DestroyObstaclesAndShips();

        SpawnShips();

        score_manager.enabled = true;
        obstacle_spawner.enabled = true;
    }

    private void DestroyObstaclesAndShips()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject object_to_destroy = transform.GetChild(i).gameObject;
            if (object_to_destroy.tag == "Asteroid" || object_to_destroy.tag == "Undestructible" || object_to_destroy.tag == "Player")
            {
                Destroy(object_to_destroy);
            }
        }
    }
}
