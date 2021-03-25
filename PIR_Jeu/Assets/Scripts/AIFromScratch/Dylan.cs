using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Dylan : MonoBehaviour
{
    public Deplacement deplacement;
    private float period = 1f;
    private float compteur = 0f;
    private int direction = 0;

    //Réseau de neuronnes
    int[] shape = { 7, 3 };
    List<float> neural_input = new List<float>();
    List<float> neural_output = new List<float>();
    List<float> biases = new List<float>();
    List<float[]> weights = new List<float[]>();
    private float reward = 0f;
    private float loss = 0f;
    private float discountFactor = 0.8f;
    private float learningRate = 0.5f;
    private float explorationRate = 1f;
    private float lastQ = 0f;


    // Start is called before the first frame update
    void Start()
    {        
        Init_neurons();
        Init_biases();
        Init_weights();
    }

    private void Init_neurons()
    {
        for (int i = 0; i < shape[0]; i++)
        {
            neural_input.Add(0f);
        }

        for (int i = 0; i < shape[1]; i++)
        {
            neural_output.Add(0f);
        }
    }

    private void Init_biases()
    {
        List<float> biasesList = new List<float>();
        for(int i = 0; i < shape[1]; i++)
        {
            biasesList.Add(Random.Range(-0.5f, 0.5f));
        }
        biases = biasesList;
    }

    private void Init_weights()
    { 
        List<float[]> weightsList = new List<float[]>();
        for (int i = 0; i < shape[1]; i++)
        {
            float[] weight = new float[shape[0]];
            for (int j = 0; j < shape[0]; j++)
            {
                weight[j] = Random.Range(0f, 1f);
            }
            weightsList.Add(weight);
        }
        weights = weightsList;
    }

    private float Activate(float x)
    {
        return ((Mathf.Exp(x)) / (Mathf.Exp(x) + 1));
    }

    private void Feed_forward()
    {
        for (int i = 0; i < shape[1]; i++)
        {
            float somme = 0;
            for (int j = 0; j < shape[0]; j++) {
                somme += neural_input[j] * weights[i][j];
            }
            neural_output[i] = Activate(somme + biases[i]);
        }
/*        Debug.Log("output 0 : " + neural_output[0]);
        Debug.Log("output 1 : " + neural_output[1]);
        Debug.Log("output 2 : " + neural_output[2]);*/
    }


    private void Backpropagate()
    {
        // TODO
    }

    // Retourne le neurone de sortie qui a la plus grande valeur
    private int Get_move()
    {
        return neural_output.IndexOf(neural_output.Max());
    }


    public void Update_reward(float r)
    {
        reward += r;
    }


    private void Compute_loss(int index)
    {
        loss = Mathf.Pow((reward + discountFactor*neural_output[index] - lastQ), 2);
        lastQ = neural_output[index];
        Debug.Log("loss is : " + loss);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        compteur += Time.fixedDeltaTime;

        if (compteur > period)
        {
            compteur = 0f;

            Update_reward(5f);
            Update_input();
            Feed_forward();
            direction = Get_move();
            Compute_loss(direction);
            reward = 0;
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

        foreach (GameObject vaisseau in vaisseaux) //Récupérer pos vaisseau
        {
            List<float> tmp = new List<float>();
            tmp.Add(vaisseau.transform.position.x);
            tmp.Add(vaisseau.transform.position.y);
            position_obstacles.Add(tmp);
        }

        List<List<float>> tmp_pos = position_obstacles;

        for(int i = 0; i < tmp_pos.Count; i++) //Boucle supprimant les astéroïdes dépassant le vaisseau
        {
            if (position_obstacles[i][1] < -5f || position_obstacles[i][1] > 0.5f)
            {
                position_obstacles.RemoveAt(i);
            }
        }

        float[] inputs = new float[7];


        for(int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = 100f;
        }

        inputs[0] = position;

        for(int i = 0; i < position_obstacles.Count; i++)
        {
            switch(position_obstacles[i][0])
            {
                case -3.61f:
                    inputs.SetValue(position_obstacles[i][1], 1);
                    break;

                case -2.18f:
                    inputs.SetValue(position_obstacles[i][1], 2);
                    break;

                case -0.78f:
                    inputs.SetValue(position_obstacles[i][1], 3);
                    break;

                case 0.58f:
                    inputs.SetValue(position_obstacles[i][1], 4);
                    break;

                case 2.03f:
                    inputs.SetValue(position_obstacles[i][1], 5);
                    break;

                case 3.49f:
                    inputs.SetValue(position_obstacles[i][1], 6);
                    break;

                default:
                    Debug.Log("Default Case");
                    break;
            }
        }
    }
}

