using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public int nb_ships = 1;

    public bool allow_weapons = true;

    public bool human_player;
    public GameOverManager game_over_manager;

    public ScoreManager score_manager;
    public ObstaclesSpawner obstacle_spawner;

    public GameObject ship_prefab;

    public GameObject piercing_shot_ui, piercing_image, cooldown_sprite, cooldown_text, health_ui;

    private List<GameObject> alive_ships;

    public Transform projectile_barrier;

    public Transform column_positions_transform;
    private List<Vector3> column_positions;

    private void Start()
    {
        //CALCULER LES POSITIONS DES COLONNES

        if (!allow_weapons)
        {
            piercing_shot_ui.SetActive(false);
            piercing_image.SetActive(false);
            cooldown_sprite.SetActive(false);
            cooldown_text.SetActive(false);
        }

        if (nb_ships > 1)
            health_ui.SetActive(false);

        alive_ships = new List<GameObject>();

        SpawnShips();
    }

    private void SpawnShips()
    {
        for (int i = 0; i < nb_ships; i++)
        {
            alive_ships.Add(Instantiate(ship_prefab, transform));
            alive_ships[alive_ships.Count - 1].GetComponentInChildren<HealthManager>().SetShipSpawner(this);
            alive_ships[alive_ships.Count - 1].GetComponentInChildren<WeaponsManager>().allow_weapons = allow_weapons;
            alive_ships[alive_ships.Count - 1].GetComponentInChildren<WeaponsManager>().projectile_barrier = projectile_barrier;
            alive_ships[alive_ships.Count - 1].GetComponentInChildren<Deplacement>().column_positions = column_positions;
        }
        if (nb_ships == 1)
        {
            alive_ships[0].GetComponentInChildren<HealthManager>().health_ui = health_ui;
            alive_ships[0].GetComponentInChildren<WeaponsManager>().piercing_shot_ui = piercing_shot_ui;
            alive_ships[0].GetComponentInChildren<WeaponsManager>().piercing_image = piercing_image;
            alive_ships[0].GetComponentInChildren<WeaponsManager>().cooldown_sprite = cooldown_sprite;
        }
    }

    public void HandleShipDeath(GameObject ship)
    {
        if (human_player)
        {
            game_over_manager.showMenu();
        }
        else
        {
            //Save this ship's score (dictionnary)

            alive_ships.Remove(ship);

            if (alive_ships.Count == 0)
            {
                Debug.Log("Reloading game");
                ReloadGame();
            }
        }
    }

    private void ReloadGame()
    {
        //Do something with the scores

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
