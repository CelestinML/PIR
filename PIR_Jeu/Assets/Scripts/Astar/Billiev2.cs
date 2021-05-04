using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billiev2 : MonoBehaviour
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

    private float UPDATE_TRAJECTORY_TIME = 0.40f;

    private float actual_move_time = 0f;
    private float actual_trajectory_time = 0f;
    private bool initialized = false;

    private int actual_position = 3;
    private int goal_position = 3;

    private float[] goal_positions;
    



    // Start is called before the first frame update
    void Start()
    {
        goal_positions = new float[6] { -3.61f, -2.18f, -0.78f, 0.58f, 2.03f, 3.49f };
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
                    if(actual_movement == MovementOrder.MOVELEFT)
                    {
                        actual_position--;
                    }
                    if(actual_movement == MovementOrder.MOVERIGHT)
                    {
                        actual_position++;
                    }
                    actual_move_time = 0;
                    actual_movement = MovementOrder.DONTMOVE;
                }
            }
        } else
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

        bool[] next_obstacles1 = new bool[6];
        bool[] next_obstacles2 = new bool[6];
        for (int i = 0; i < 6; ++i)
        {
            next_obstacles1[i] = false;
            next_obstacles2[i] = false;
        }


        float lowest1 = 10f;
        float lowest2 = 10f;
        foreach (Vector3 obs in position_obstacles)
        {
            if (obs.y < lowest1 && obs.y > gameObject.transform.position.y)
            {
                lowest1 = obs.y;
            }
            if (obs.y > lowest1 && obs.y < lowest2)
            {
                lowest2 = obs.y;
            }
        }
        Debug.Log(lowest1);
        Debug.Log(lowest2);

        foreach (Vector3 obs in position_obstacles)
        {
            //Debug.Log(obs.x);
            if (obs.y == lowest1)
            {
                switch (obs.x)
                {
                    case -3.61f:
                        next_obstacles1[0] = true;
                        break;
                    case -2.18f:
                        next_obstacles1[1] = true;
                        break;
                    case -0.78f:
                        next_obstacles1[2] = true;
                        break;
                    case 0.58f:
                        next_obstacles1[3] = true;
                        break;
                    case 2.03f:
                        next_obstacles1[4] = true;
                        break;
                    case 3.49f:
                        next_obstacles1[5] = true;
                        break;
                }
            } else
            {
                if (obs.y == lowest2)
                {
                    Debug.Log("COUCOU");
                    switch (obs.x)
                    {
                        case -3.61f:
                            next_obstacles2[0] = true;
                            break;
                        case -2.18f:
                            next_obstacles2[1] = true;
                            break;
                        case -0.78f:
                            next_obstacles2[2] = true;
                            break;
                        case 0.58f:
                            next_obstacles2[3] = true;
                            break;
                        case 2.03f:
                            next_obstacles2[4] = true;
                            break;
                        case 3.49f:
                            next_obstacles2[5] = true;
                            break;
                    }
                }
            }
        }

        // Computes where is the nearest place we can go
        int[] scores = new int[6];
        
        for(int i=0; i<6; ++i) // Iterating through the first line
        { 
            if (! next_obstacles1[i])
            {
                int current_position_score_line_1 = compute_score(i);
                if(current_position_score_line_1 != 0)
                {
                    bool dontmove2 = false;
                    int maximum_score_line_2 = 0;
                    for(int j=0; j<6; ++j) // Iterating through the second line
                    {
                        if (! next_obstacles2[j])
                        {
                            int current_position_score_line_2 = compute_score(j);
                            if (maximum_score_line_2 < current_position_score_line_2)
                            {
                                maximum_score_line_2 = current_position_score_line_2;
                                if (i == j)
                                {
                                    dontmove2 = true;
                                } else
                                {
                                    dontmove2 = false;
                                }
                            }
                        }
                    }
                    scores[i] = current_position_score_line_1 * maximum_score_line_2 + (dontmove2 ? 1000:0);
                }
            }
        }

        int whereToGo = 6;
        int maximum_score = -1;
        for(int i=0; i<6; i++)
        {
            if(scores[i] > maximum_score)
            {
                Debug.Log("YEEEEEEEEE");
                maximum_score = scores[i];
                whereToGo = i - actual_position;
            }
        }
        if(maximum_score == 0)
        {
            whereToGo = 0;
        }

        /*

        if( maximum_score == 484)
        // 484 corresponds to a move on side and a move front.
        // We want to move on side to occur first if possible
        {
            if(scores[Mathf.Min(actual_position+1,5)] == 484 && next_obstacles1[actual_position+1] == false)
            {
                whereToGo = 1;
            } else
            {
                if(scores[Mathf.Max(actual_position - 1, 0)] == 484 && next_obstacles1[actual_position - 1] == false)
                {
                    whereToGo = -1;
                }
            }
        }
        */


        if(actual_position <= 2)
        {
            if(whereToGo == 0)
            // Ai considered no move was needed
            {
                for (int i = actual_position+1; i <= 2; ++i)
                {
                    if( next_obstacles1[i] == false)
                    {
                        whereToGo = i - actual_position;
                    } else
                    {
                        break;
                    }
                }
            }   
        }


        if (actual_position >= 3)
        {
            if (whereToGo == 0)
            // Ai considered no move was needed
            {
                for (int i = actual_position -1; i >= 3; --i)
                {
                    if (next_obstacles1[i] == false)
                    {
                        whereToGo = i - actual_position;
                    } else
                    {
                        // asteroid will crush the way
                        break;
                    }
                }
            }

        }


        string debug = "line1: [";
        for (int i = 0; i < 6; ++i)
        {
            debug += next_obstacles1[i].ToString() + " ,";
        }
        debug += "]\n";
        debug += "line2: [";
        for (int i = 0; i < 6; ++i)
        {
            debug += next_obstacles2[i].ToString() + " ,";
        }
        debug += "scores: [";
        for (int i = 0; i < 6; ++i)
        {
            debug += scores[i].ToString() + " ,";
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


    int compute_score(int position)
    {
        int score = Mathf.Max(0, 10 * (3 - Mathf.Abs(position - actual_position)));
        if (position == 0 || position == 5) return 1 + score;
        if (position == 1 || position == 4) return 2 + score;
        if (position == 2 || position == 3) return 3 + score;
        return score;
    }

}


