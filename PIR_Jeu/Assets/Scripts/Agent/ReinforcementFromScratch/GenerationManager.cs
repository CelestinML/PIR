using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    // Generation management 
    private UpdateGeneration generationUpdator;

    // Array of neural networks
    public Kevin[] generation;

    private int nb_ships;
    private int alive_ships;

    private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Debut awake");
        scoreManager = transform.parent.GetComponentInChildren<ScoreManager>();
        Debug.Log("Apres score manager");
        nb_ships = MenuAI.nb_ships;
        alive_ships = nb_ships;
        Debug.Log("Avant new");
        generationUpdator = new UpdateGeneration();
        Debug.Log("Après new");
        generation = generationUpdator.CreateGeneration();
    }

    public void NotifyDeath(int index)
    {
        generationUpdator.GetChildScore(index, scoreManager.score);

        alive_ships--;

        if (alive_ships == 0)
        {
            generationUpdator.HandleEndOfGeneration();
        }
    }


    
}
