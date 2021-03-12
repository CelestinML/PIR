using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;

public class DodgeObstaclesAgent : Agent
{
    public Deplacement deplacement;
    public InfosVaisseau infosVaisseau;
    public ObstaclesSpawner obstaclesSpawner;

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(0, -4.013922f, 0);
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        GameObject[] vaisseaux = GameObject.FindGameObjectsWithTag("Undestructible");
        foreach (GameObject asteroid in asteroids)
        {
            Destroy(asteroid);
        }
        foreach (GameObject vaisseau in vaisseaux)
        {
            Destroy(vaisseau);
        }
        infosVaisseau.score = 0;
        infosVaisseau.vies_ui.GetComponent<TextMeshProUGUI>().text = "Vies : 3";
        infosVaisseau.points_de_vie = 3;
        obstaclesSpawner.max_fall_speed = 3;
        obstaclesSpawner.spawn_period = 2;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        GameObject[] vaisseaux = GameObject.FindGameObjectsWithTag("Undestructible");
        foreach(GameObject asteroid in asteroids)
        {
            sensor.AddObservation(asteroid.transform.position);
        }
        foreach (GameObject vaisseau in vaisseaux)
        {
            sensor.AddObservation(vaisseau.transform.position);
        }

    }
   
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        if (Input.GetAxisRaw("Horizontal") < 0) {
            discreteActions[0] = 1;
         }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            discreteActions[0] = 2;
        }
        else
        {
            discreteActions[0] = 0;
        }
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];
        if(action == 0)
        {
            deplacement.DoNotMove();
        }
        else if(action == 1)
        {
            deplacement.MoveLeft();
        }
        else
        {
            deplacement.MoveRight();
        }
    }
    
}
