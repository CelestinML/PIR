using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billiev0 : MonoBehaviour
{
    // Defines possible movements.
    private enum MovementOrder { DONTMOVE, MOVELEFT, MOVERIGHT };

    public Deplacement deplacement;

    // Variable storing the actual movement we are doing.
    private MovementOrder actual_movement = MovementOrder.MOVERIGHT;


    // Defines how long is movement to do a discret bound. 
    private float MOVE_TIME = 0.30f;
    // Defines how long is movement to initialize position.
    private float INITIALIZE_MOVE_TIME = 0.14f;

    private float UPDATE_TRAJECTORY_TIME = 0.35f;

    private float actual_move_time = 0f;
    private float actual_trajectory_time = 0f;
    private bool initialized = false;

    private int actual_position = 3;
    private int goal_position = 3;





    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        actual_trajectory_time += Time.fixedDeltaTime;

        if (actual_trajectory_time >= UPDATE_TRAJECTORY_TIME)
        {
            computeMovement();
            actual_trajectory_time = 0;
        }

        if (initialized)
        {
            // Main IA loop
            if (actual_movement != MovementOrder.DONTMOVE)
            {
                actual_move_time += Time.fixedDeltaTime;
                // If the movement has been long enough
                if (actual_move_time >= MOVE_TIME)
                {
                    if (actual_movement == MovementOrder.MOVELEFT)
                    {
                        actual_position--;
                    }
                    if (actual_movement == MovementOrder.MOVERIGHT)
                    {
                        actual_position++;
                    }
                    actual_move_time = 0;
                    actual_movement = MovementOrder.DONTMOVE;
                }
            }
        }
        else
        {
            actual_move_time += Time.fixedDeltaTime;
            if (actual_move_time >= INITIALIZE_MOVE_TIME)
            {
                initialized = true;
                actual_move_time = 0;
                actual_movement = MovementOrder.DONTMOVE;
                Debug.Log("=====INITIALIZED=====");
            }
        }



        switch (actual_movement)
        {
            case MovementOrder.DONTMOVE:
                deplacement.DoNotMove();
                break;
            case MovementOrder.MOVELEFT:
                deplacement.MoveLeft();
                break;
            case MovementOrder.MOVERIGHT:
                deplacement.MoveRight();
                break;
            default:
                Debug.Log("ERROR HAVE BEEN MADE ON 'actual_movement' variable");
                break;
        }




        //Debug.Log(actual_movement);


    }

    void computeMovement()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        GameObject[] vaisseaux = GameObject.FindGameObjectsWithTag("Undestructible");

        List<Vector3> position_obstacles = new List<Vector3>();


        foreach (GameObject asteroid in asteroids)
        {
            position_obstacles.Add(asteroid.transform.position);
        }
        foreach (GameObject vaisseau in vaisseaux)
        {
            position_obstacles.Add(vaisseau.transform.position);
        }

        bool[] next_obstacles = new bool[6];
        for (int i = 0; i < 6; ++i)
        {
            next_obstacles[i] = false;
        }


        float lowest = 10f;
        foreach (Vector3 obs in position_obstacles)
        {
            if (obs.y < lowest && obs.y > gameObject.transform.position.y)
            {
                lowest = obs.y;
            }
        }

        foreach (Vector3 obs in position_obstacles)
        {
            //Debug.Log(obs.x);
            if (obs.y == lowest)
            {
                switch (obs.x)
                {
                    case -3.61f:
                        next_obstacles[0] = true;
                        break;
                    case -2.18f:
                        next_obstacles[1] = true;
                        break;
                    case -0.78f:
                        next_obstacles[2] = true;
                        break;
                    case 0.58f:
                        next_obstacles[3] = true;
                        break;
                    case 2.03f:
                        next_obstacles[4] = true;
                        break;
                    case 3.49f:
                        next_obstacles[5] = true;
                        break;
                }
            }
        }

        // Computes where is the nearest place we can go
        int whereToGo = 6;
        int score;


        for (int i = 0; i < 6; ++i)
        {
            //Debug.Log("next_obstacles " + i.ToString() + " is "  + next_obstacles[i].ToString());
            if (next_obstacles[i] == false)
            {
                score = i - actual_position;
                // Selection of minimal score
                if (Mathf.Abs(score) < Mathf.Abs(whereToGo))
                {
                    whereToGo = score;
                }
            }
        }

        string debug = "[";
        for (int i = 0; i < 6; ++i)
        {
            debug += next_obstacles[i].ToString() + " ,";
        }
        Debug.Log(debug);


        if (whereToGo < 0)
        {
            actual_movement = MovementOrder.MOVELEFT;
        }
        else
        {
            if (whereToGo > 0)
            {
                actual_movement = MovementOrder.MOVERIGHT;
            }
            else
            {
                actual_movement = MovementOrder.DONTMOVE;
            }
        }
    }
}
