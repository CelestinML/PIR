using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    //INPUT :
    public List<GameObject> asteroids;
    /*
     * Coordonnées : asteroids[index].transform.position
     * Vitesse : asteroids[index].GetComponent<ChuteAsteroide>().falling_speed
     */

    //OUTPUT :
    //L'agent va effectuer des actions sur le vaisseau
    public GameObject vaisseau;
    /*
     * Trois actions sont possibles :
     * - MoveLeft()
     * - MoveRight()
     * - DoNotMove()
     */

    //Cette méthode sera appelée dès que tous les éléments nécessaires au jeu seront chargés, avant que la première frame se lance
    private void Start()
    {
        //On doit instancier notre liste d'astéroides
        //On y ajoutera les astéroides dès qu'on les crée dans ObstaclesSpawner
        //Je ne sais pas encore si ils seront supprimés de la liste automatiquement quand ils seront détruits
        asteroids = new List<GameObject>();
    }

    //Cette méthode sera appelée automatiquement chaque frame, c'est probablement ici qu'on va faire apprendre notre agent et déterminer les outputs
    private void FixedUpdate()
    {
        
    }
}
