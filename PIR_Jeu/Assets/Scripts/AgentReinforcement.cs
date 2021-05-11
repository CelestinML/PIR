using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;

public class AgentReinforcement : Agent
{
    public Movement movement;

    public Overlap overlap;

    public RaycastObservation raycastObservation;

    private Transform ship;
    public Transform left_border, right_border;

    public override void OnEpisodeBegin()
    {
        left_border = transform.parent.parent.GetComponent<ShipSpawner>().border_left;
        right_border = transform.parent.parent.GetComponent<ShipSpawner>().border_right;

        ship = transform.parent;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        //Observations V1
        /*
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
        sensor.AddObservation(bord_gauche);
        sensor.AddObservation(bord_droit);
        */

        //Observations V2

        //We show the agent the ship's position
        sensor.AddObservation(ship.localPosition.x);
        sensor.AddObservation(ship.localPosition.y);
        //Debug.Log("Ship : " + ship.localPosition.x);

        //We show the agent the border's positions
        sensor.AddObservation(left_border.localPosition.x);
        sensor.AddObservation(right_border.localPosition.x);
        /*List<GameObject> obstacles = overlap.GetObstacles();
        foreach (GameObject obstacle in obstacles)
            sensor.AddObservation(Vector3.Distance(ship.position, obstacle.transform.localPosition));*/

        List<Vector3> obstacles_infos = raycastObservation.GetPositions();
        foreach (Vector3 info in obstacles_infos)
        {
            sensor.AddObservation(info);
            //Debug.Log("Info : " + info);
        }

    }

    /*public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
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
    }*/

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];
        if (action == 0)
        {
            Debug.Log("Did not move");
            movement.DoNotMove();
        }
        else if (action == 1)
        {
            Debug.Log("Moved left");
            movement.MoveLeft();
        }
        else
        {
            Debug.Log("Moved right");
            movement.MoveRight();
        }
    }
}
