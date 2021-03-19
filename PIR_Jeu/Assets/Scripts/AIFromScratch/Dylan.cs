using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dylan : MonoBehaviour
{
    public Deplacement deplacement;
    private float period_deplacement = 0.2f;
    private float period_acquisition = 0.5f;
    private float compteur_deplacement = 0f;
    private float compteur_acquisition = 0f;
    private int direction = 0;
    private float reward = 0f;

    //Réseau de neuronnes
    List<float> neural_input = new List<float>();
    List<float> neural_output = new List<float>();
    List<float> biases = new List<float>();
    List<float> weights = new List<float>();


    // Start is called before the first frame update
    void Start()
    {        
        Init_neurons();
        Init_biases();
        Init_weights();
    }

    private void Init_neurons()
    {
        for (int i = 0; i < 7; i++)
        {
            neural_input.Add(0f);
        }

        for (int i = 0; i < 3; i++)
        {
            neural_output.Add(Random.Range(0f, 1f));
        }
    }

    private void Init_biases()
    {
        for (int i = 0; i < 7; i++)
        {
            biases.Add(Random.Range(-0.5f, 0.5f));
        }
    }

    private void Init_weights()
    {
        for (int i = 0; i < 7; i++)
        {
            biases.Add(Random.Range(0f, 1f));
        }
    }

    private float Activate(float x)
    {
        return ((Mathf.Exp(x)) / (Mathf.Exp(x) + 1));
    }

    /*private float Feed_forward()
    {
        foreach (float neuron in neural_input)
        {

        }
    }*/


    public void Update_reward(float r)
    {
        reward += r;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        compteur_deplacement += Time.fixedDeltaTime;
        compteur_acquisition += Time.fixedDeltaTime;
        if (compteur_deplacement > period_deplacement)
        {
            
            Update_reward(1f);
            compteur_deplacement = 0f;
            direction = Random.Range(0, 3);
            Debug.Log("Le reward total est de : " + reward);
        }

        if (compteur_acquisition > period_acquisition) {
            compteur_acquisition = 0f;
            Update_input();
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
            Debug.Log("La position de cet asteroïde est de : {" + position_obstacles[i][0] + "; " +  position_obstacles[i][1] + "}");
            if (position_obstacles[i][1] < -5f || position_obstacles[i][1] > 0.5f)
            {
                position_obstacles.RemoveAt(i);
                Debug.Log("Element supprimé, la taille du tableau est de : " + position_obstacles.Count);  
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
        for(int i = 0; i < inputs.Length; i++)
        {
            Debug.Log("Input[" + i + "] = " + inputs[i]);
        }
    }
}

