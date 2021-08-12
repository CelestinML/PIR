using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCalculator : MonoBehaviour
{
    public float raycast_distance = 4f;

    public Transform raycast_left, raycast_center, raycast_right;

    public LayerMask layerMask;

    private AgentReinforcement agent;

    private float no_hit_reward = 1f;
    private float max_hit_bad_reward = -5f;

    private HealthManager healthManager;

    private int health_points;

    private void Start()
    {
        healthManager = transform.parent.parent.GetComponentInChildren<HealthManager>();
        health_points = healthManager.points_de_vie;

        agent = GetComponent<AgentReinforcement>();
    }

    void Update()
    {
        RaycastHit2D hit_left = Physics2D.Raycast(raycast_left.position, Vector2.up, raycast_distance, layerMask);
        //Debug.DrawLine(raycast_left.position, raycast_left.position + Vector3.up * raycast_distance, Color.green);
        RaycastHit2D hit_center = Physics2D.Raycast(raycast_center.position, Vector2.up, raycast_distance, layerMask);
        //Debug.DrawLine(raycast_center.position, raycast_center.position + Vector3.up * raycast_distance, Color.green);
        RaycastHit2D hit_right = Physics2D.Raycast(raycast_right.position, Vector2.up, raycast_distance, layerMask);
        //Debug.DrawLine(raycast_right.position, raycast_right.position + Vector3.up * raycast_distance, Color.green);

        if (hit_left.collider == null && hit_center.collider == null && hit_right.collider == null)
        {
            Debug.Log("Raycast hit");
            agent.AddReward(no_hit_reward);
        }
        else
        {
            Debug.Log("Raycast no hit");
            agent.AddReward(max_hit_bad_reward);
        }

        /*if (hit_left.collider != null)
        {
            reward += max_hit_bad_reward * (raycast_distance - hit_left.distance) / raycast_distance;
            //Debug.Log("HitReward left : " + hit_left.transform.localPosition);
            agent.AddReward(max_hit_bad_reward * (raycast_distance - hit_left.distance) / raycast_distance);
        }
        else
        {
            reward += no_hit_reward;
            agent.AddReward(no_hit_reward);
        }

        if (hit_center.collider != null)
        {
            reward += max_hit_bad_reward * (raycast_distance - hit_center.distance) / raycast_distance;
            //Debug.Log("HitReward center : " + hit_center.transform.localPosition);
            agent.AddReward(max_hit_bad_reward * (raycast_distance - hit_center.distance) / raycast_distance);
        }
        else
        {
            reward += no_hit_reward;
            agent.AddReward(no_hit_reward);
        }

        if (hit_right.collider != null)
        {
            reward += max_hit_bad_reward * (raycast_distance - hit_right.distance) / raycast_distance;
            //Debug.Log("HitReward right : " + hit_right.transform.localPosition);
            agent.AddReward(max_hit_bad_reward * (raycast_distance - hit_right.distance) / raycast_distance);
        }
        else
        {
            reward += no_hit_reward;
            agent.AddReward(no_hit_reward);
        }*/

        //Debug.Log("Reward for this frame : " + reward);

        if (health_points != healthManager.points_de_vie)
        {
            Debug.Log("Ship was hit");
            agent.AddReward(-20f);
            health_points--;
        }
    }
}
