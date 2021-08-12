using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public ObstaclesSpawner obstacle_spawner;

    private float score;

    private float base_spawn_period, base_max_fall_speed;

    //Infos pour booster la difficulté en fonction du score
    private bool period_boost_block = false;
    private List<int> boost_moments;
    private bool max_speed_boost_block = false;
    private List<int> max_speed_boost_moments;

    private void Start()
    {
        base_spawn_period = obstacle_spawner.spawn_period;
        base_max_fall_speed = obstacle_spawner.max_fall_speed;

        boost_moments = new List<int>();
        boost_moments.Add(200);
        boost_moments.Add(400);
        boost_moments.Add(800);
        boost_moments.Add(2000);
        boost_moments.Add(6000);
        max_speed_boost_moments = new List<int>();
        max_speed_boost_moments.Add(1500);
        max_speed_boost_moments.Add(3000);
        max_speed_boost_moments.Add(10000);
    }

    public void ResetDifficulty()
    {
        obstacle_spawner.spawn_period = base_spawn_period;
        obstacle_spawner.max_fall_speed = base_max_fall_speed;
    }

    public void SetScore(float score)
    {
        this.score = score;
    }

    private void FixedUpdate()
    {
        //On détermine si un boost de période de spawn doit être fait
        if (boost_moments.Contains(Mathf.FloorToInt(score)))
        {
            if (!period_boost_block)
            {
                obstacle_spawner.spawn_period *= 0.8f;
                period_boost_block = true;
            }
        }
        else if (period_boost_block)
            period_boost_block = false;

        //On détermine si un boost de vitesse max de chute doit être fait
        if (max_speed_boost_moments.Contains(Mathf.FloorToInt(score)))
        {
            if (!max_speed_boost_block)
            {
                obstacle_spawner.max_fall_speed *= 1.5f;
                max_speed_boost_block = true;
            }
        }
        else if (max_speed_boost_block)
            max_speed_boost_block = false;
    }
}
