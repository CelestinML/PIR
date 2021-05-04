using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public GameObject environment_prefab;

    public int nb_environments = 1;
    public bool human_player = true;
    public int nb_ships_per_environment = 1;
    public bool allow_weapons = true;
    public bool activate_bonuses = true;

    private List<Vector3> spawn_points;

    private Vector3 up_left = Vector3.zero, up_right = Vector3.zero, down_left = Vector3.zero, down_right = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        spawn_points = new List<Vector3>();

        Transform spawns = transform.GetChild(0);
        for (int i = 0; i < spawns.childCount; i++)
        {
            spawn_points.Add(spawns.GetChild(i).position);
        }

        SpawnEnvironments();
    }

    private void SpawnEnvironments()
    {
        if (human_player)
        {
            Debug.Log("If you choose a human player, there can only be one environment and one ship per environment");
            nb_environments = 1;
            nb_ships_per_environment = 1;
        }
        for (int i = 0; i < nb_environments; i++)
        {
            GameObject environment = Instantiate(environment_prefab, spawn_points[i], Quaternion.identity);
            environment.GetComponent<ShipSpawner>().human_player = human_player;
            environment.GetComponent<ShipSpawner>().nb_ships = nb_ships_per_environment;
            environment.GetComponent<ShipSpawner>().allow_weapons = allow_weapons;
            environment.GetComponentInChildren<BonusManager>().activate_bonuses = activate_bonuses;

            //Calculate the borders
            Transform positions = environment.transform.Find("BorderPositions");
            for (int j = 0; j < positions.childCount; j++)
            {
                //Debug.Log(positions.GetChild(j).position);
                up_left.x = Mathf.Min(up_left.x, positions.GetChild(j).position.x);
                up_left.y = Mathf.Max(up_left.y, positions.GetChild(j).position.y);
                up_right.x = Mathf.Max(up_right.x, positions.GetChild(j).position.x);
                up_right.y = Mathf.Max(up_right.y, positions.GetChild(j).position.y);
                down_left.x = Mathf.Min(down_left.x, positions.GetChild(j).position.x);
                down_left.y = Mathf.Min(down_left.y, positions.GetChild(j).position.y);
                down_right.x = Mathf.Max(down_right.x, positions.GetChild(j).position.x);
                down_right.y = Mathf.Min(down_right.y, positions.GetChild(j).position.y);
            }
        }

        Debug.Log(up_right);

        //Adapt the camera
        Camera.main.transform.position = new Vector3((up_right.x + up_left.x) / 2f, (up_left.y + down_left.y) / 2f, -10);

        while (Camera.main.ScreenToWorldPoint(new Vector3(1, 1, 0)).x > up_left.x)
        {
            Camera.main.orthographicSize += 0.5f;
        }
    }
}
