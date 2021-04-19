using System.Collections.Generic;
using UnityEngine;

public class ShipSpawnerTraining : MonoBehaviour
{
    // generation parameter and dead vaisseaux tracker
    public int nbVaisseauToSpawn = 100;
    private int deadVaisseau = 0;

    // gameObject init
    public Transform vaisseauTransform;
    public GameObject vaisseauModel;
    private List<GameObject> vaisseaux;

    // Allow to control the time scale
    public float timeScale = 1F;

    // Obstacles spawner instance
    private ObstaclesSpawnerTraining obstaclesSpawner;

    // Generation management 
    private UpdateGeneration generationUpdator;

    // Array of neural networks
    private Kevin[] generation;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScale;
        obstaclesSpawner = GetComponent<ObstaclesSpawnerTraining>();

        generationUpdator = new UpdateGeneration();
        generation = generationUpdator.CreateGeneration();
        InitVaisseaux();
    }


    public void InitVaisseaux()
    {
        vaisseaux = new List<GameObject>();

        for (int i = 0; i < nbVaisseauToSpawn; i++)
        {
            vaisseaux.Add(Instantiate(vaisseauModel, vaisseauTransform.position, Quaternion.identity));
            vaisseaux[i].GetComponent<MoveWithAI>().UpdateAI(generation[i]);
        }
    }


    public void UpdateChildScore(GameObject vaisseau, float score)
    {
        lock(this)
        {
            int index = vaisseaux.IndexOf(vaisseau);
            generationUpdator.GetChildScore(index, score);

            deadVaisseau++;

            if (deadVaisseau == nbVaisseauToSpawn)
            {
                for (int i = 0; i < nbVaisseauToSpawn; i++)
                {
                    Destroy(vaisseaux[i]);
                }
                deadVaisseau = 0;
                ReloadGame();
            }
        }
    }


    private void ReloadGame()
    {
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

        obstaclesSpawner.max_fall_speed = 3;
        obstaclesSpawner.spawn_period = 2;

        generation = generationUpdator.HandleEndOfGeneration();
        obstaclesSpawner.Reset();
        InitVaisseaux();
    }
}
