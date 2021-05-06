using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MoveWithAI : MonoBehaviour
{
    public Deplacement deplacement;
    private float period = 0.1f;
    private float compteur = 0f;
    private int direction = 0;

    private Kevin kevin;

    private float[] inputs;
    private float[] outputs;

    public Transform raycastLeft, raycastMiddle, raycastRight; // origine des raycast
    public LayerMask layerMask; // quel est le layer de collision pour le raycast

    // Start is called before the first frame update
    void Start()
    {
        inputs = new float[9];
        outputs = new float[3];
    }


    public void UpdateAI(Kevin pKevin)
    {
        kevin = pKevin;
    }

    public Kevin GetAI()
    {
        return kevin;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        compteur += Time.fixedDeltaTime;

        if (compteur > period)
        {
            compteur = 0f;

            Update_Inputs_Raycast();
            outputs = kevin.FeedForward(inputs);

/*            for (int i = 0; i < inputs.Length; i++)
            {
                Debug.Log("inputs " + i + " : " + inputs[i]);
            }

            for (int i = 0; i < outputs.Length; i++)
            {
                Debug.Log("outputs " + i + " : " + outputs[i]);
            }*/

            direction = GetMove();
        }

        if (direction == 0)
        {
            deplacement.DoNotMove();
        }

        else if (direction == 1)
        {
            deplacement.MoveLeft();
        }

        else
        {
            deplacement.MoveRight();
        }
    }


    private int GetMove()
    {
        List<float> tmpOutputs = new List<float>(outputs);
        return tmpOutputs.IndexOf(tmpOutputs.Max());
    }


    public void Update_Inputs_Raycast()
    {
        List<RaycastHit2D> raycasts = new List<RaycastHit2D>();

        raycasts.Add(Physics2D.Raycast(raycastMiddle.position, Vector2.up, 10f, layerMask)); // middle up shot

        raycasts.Add(Physics2D.Raycast(raycastLeft.position, Vector2.left, 10f, layerMask)); // left left shot
        raycasts.Add(Physics2D.Raycast(raycastRight.position, Vector2.right, 10f, layerMask)); // right right shot

        raycasts.Add(Physics2D.Raycast(raycastLeft.position, Vector2.up, 10f, layerMask)); // left up shot
        raycasts.Add(Physics2D.Raycast(raycastRight.position, Vector2.up, 10f, layerMask)); // right up shot

        raycasts.Add(Physics2D.Raycast(raycastMiddle.position, Quaternion.Euler(0, 0, -10) * Vector2.up, 10f, layerMask)); // 10 degrees middle left shot
        raycasts.Add(Physics2D.Raycast(raycastMiddle.position, Quaternion.Euler(0, 0, -10) * Vector2.up, 10f, layerMask)); // 10 degrees middle right shot

        raycasts.Add(Physics2D.Raycast(raycastMiddle.position, Quaternion.Euler(0, 0, -20) * Vector2.up, 10f, layerMask)); // 20 degrees middle left shot
        raycasts.Add(Physics2D.Raycast(raycastMiddle.position, Quaternion.Euler(0, 0, -20) * Vector2.up, 10f, layerMask)); // 20 degrees middle right shot

        for (int i = 0; i < raycasts.Count; i++)
        {
            if (raycasts[i].collider != null)
            {
                inputs[i] = raycasts[i].distance;
            }
            else
            {
                inputs[i] = 10f;
            }
        }
    }

    void Update_input()
    {
        float position = gameObject.transform.position.x;
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        GameObject[] vaisseaux = GameObject.FindGameObjectsWithTag("Undestructible");
        List<List<float>> position_obstacles = new List<List<float>>();

        foreach (GameObject asteroid in asteroids) //Récupérer pos astéroïdes
        {
            List<float> tmp = new List<float>();
            tmp.Add(asteroid.transform.position.x);
            tmp.Add(asteroid.transform.position.y);
            position_obstacles.Add(tmp);
        }

        foreach (GameObject vaisseau in vaisseaux) // Récupérer pos vaisseau
        {
            List<float> tmp = new List<float>();
            tmp.Add(vaisseau.transform.position.x);
            tmp.Add(vaisseau.transform.position.y);
            position_obstacles.Add(tmp);
        }

        List<List<float>> tmp_pos = position_obstacles;

        for (int i = 0; i < tmp_pos.Count; i++) // Boucle supprimant les astéroïdes dépassant le vaisseau
        {
            if (position_obstacles[i][1] < -5f || position_obstacles[i][1] > 0.5f)
            {
                position_obstacles.RemoveAt(i);
            }
        }

        float[] currentInputs = new float[7];


        for (int i = 0; i < currentInputs.Length; i++)
        {
            currentInputs[i] = 10f;
        }

        currentInputs[0] = position;
        

        // Get the column for each asteroïd
        for (int i = 0; i < position_obstacles.Count; i++)
        {
            switch (position_obstacles[i][0])
            {
                case -3.61f:
                    currentInputs.SetValue(position_obstacles[i][1], 1);
                    break;

                case -2.18f:
                    currentInputs.SetValue(position_obstacles[i][1], 2);
                    break;

                case -0.78f:
                    currentInputs.SetValue(position_obstacles[i][1], 3);
                    break;

                case 0.58f:
                    currentInputs.SetValue(position_obstacles[i][1], 4);
                    break;

                case 2.03f:
                    currentInputs.SetValue(position_obstacles[i][1], 5);
                    break;

                case 3.49f:
                    currentInputs.SetValue(position_obstacles[i][1], 6);
                    break;

                default:
                    Debug.Log("Default Case");
                    break;
            }
        }
        inputs = currentInputs;
    }
}
