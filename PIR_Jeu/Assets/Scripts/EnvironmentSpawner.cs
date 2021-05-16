using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public static List<float> scores = new List<float>();

    public GameObject environment_prefab;

    public int nb_environments = 1;
    public bool human_player = true;
    public int nb_ships_per_environment = 1;
    public bool allow_weapons = true;
    public bool activate_bonuses = true;
    public string ai_type;

    private List<Vector3> spawn_points;

    private Vector3 up_left = Vector3.zero, up_right = Vector3.zero, down_left = Vector3.zero, down_right = Vector3.zero;

    public List<GameObject> agents;


    // Start is called before the first frame update
    void Start()
    {
if (mainMenu.human_player)
        {
            human_player = true;
            nb_environments = 1;
            allow_weapons = true;
            nb_ships_per_environment = 1;
        }
        else
        {
            //We fetch the options chosen by the user at the start
            human_player = false;
            nb_environments = MenuAI.nb_environments;
            allow_weapons = MenuAI.allow_weapons;
            nb_ships_per_environment = MenuAI.nb_ships;
            ai_type = MenuAI.ai;
        }

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
        for (int i = 0; i < nb_environments; i++)
        {
            GameObject environment = Instantiate(environment_prefab, spawn_points[i], Quaternion.identity);
            environment.GetComponent<ShipSpawner>().human_player = human_player;
            environment.GetComponent<ShipSpawner>().nb_ships = nb_ships_per_environment;
            environment.GetComponent<ShipSpawner>().allow_weapons = allow_weapons;
            if (!human_player)
                environment.GetComponent<ShipSpawner>().agent = FindAgent();

            environment.GetComponentInChildren<BonusManager>().activate_bonuses = activate_bonuses;

            //Calculate the borders
            Transform positions = environment.transform.Find("CornerPositions");
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

        //Adapt the camera
        Camera.main.transform.position = new Vector3((up_right.x + up_left.x) / 2f, (up_left.y + down_left.y) / 2f, -10);

        while (Camera.main.ScreenToWorldPoint(new Vector3(1, 1, 0)).x > up_left.x)
        {
            Camera.main.orthographicSize += 0.5f;
        }
        if (nb_environments == 12)
        {
            Camera.main.orthographicSize = 28f;
        }
    }

    private GameObject FindAgent()
    {
        string agent_name = null;
        if (ai_type == "ML-Agents")
        {
            agent_name = "AgentReinforcement";
        }
        else if(ai_type == "A*")
        {
            agent_name = "AgentA";
        }
        else if(ai_type == "Evolutionary methods")
        {
            agent_name = "AgentEvolutionary";
        }
        foreach (GameObject agent in agents)
        {
            if (agent.name == agent_name)
            {
                return agent;
            }
        }
        throw new Exception();
    }

    private void OnDestroy()
    {
        StoreScore();
    }

    public void StoreScore()
    {
        String str = "";

        string path = Directory.GetCurrentDirectory() + "/Files/Score.txt";

        for (int i = 0; i < scores.Count; i++)
        {
            str = str + scores[i].ToString();
            str += "\n";
            if ((i+1) % nb_ships_per_environment == 0)
            {
                str += "*---*\n";
            }
        }

        if (!File.Exists(path))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                Debug.Log("No file found");
            }
        }

        // This text is always added, making the file longer over time
        // if it is not deleted.
        using (StreamWriter sw = new StreamWriter(path, true))
        {
            sw.WriteLine(str);
        }

        // Open the file to read from.
        /*using (StreamReader sr = File.OpenText(path))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                Console.WriteLine(s);
            }
        }*/

    }
}
