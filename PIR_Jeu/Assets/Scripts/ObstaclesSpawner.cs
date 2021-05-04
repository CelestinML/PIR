using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    public GameObject asteroid;
    public GameObject ennemy_ship;
    public BonusManager bonus_manager;

    [Range(1, 5)]
    public int max_asteroids = 4;

    //The obstacles'speed will be randomized between the min and the max value
    //You should set the min and the max value to the same base value as the max value
    //will increase faster with the difficulty boosts
    public float min_fall_speed = 3;
    public float max_fall_speed = 3;

    //The asteroids'rotation speed will be randomized between the min and the max value
    public float min_rotation_speed = 1;
    public float max_rotation_speed = 4;

    //Internal variable to count time between obstacles spawns
    private float time_since_last_spawn = 0;
    //Original time between two obstacles spawns
    //This time will be reduced with difficulty boosts
    public float spawn_period = 2;

    //The positions on which the obstacles will spawn
    private List<Vector3> spawn_points = new List<Vector3>();
    //Once the obstacles will be under this barrier, they will be detroyed
    private Vector3 barrier;

    private Transform environment;

    // Start is called before the first frame update
    void Start()
    {
        //We get the barrier's position
        barrier = transform.GetChild(0).localPosition;

        //We get the obstacle spawn positions
        Transform spawns = transform.GetChild(1);
        for (int i = 0; i < spawns.childCount; i++)
        {
            spawn_points.Add(spawns.GetChild(i).localPosition);
        }

        //We get the environment's transform to set it as the obstacles'parent
        environment = transform.parent;
    }

    private void FixedUpdate()
    {
        //We count the elapsed time since the last obstacle spawn
        time_since_last_spawn += Time.fixedDeltaTime;
        if (time_since_last_spawn > spawn_period)
        {
            //If this time is greater than the spawn period, we set it back to 0 and spawn asteroids
            SpawnAsteroids();
            time_since_last_spawn = 0;
        }
    }

    private void SpawnAsteroids()
    {
        //On fait apparaître entre 1 et max_asteroids astéroides par ligne
        int number_of_asteroids = Random.Range(1, max_asteroids); //A chaque spawn d'asteroides, on détermine aléatoirement le nombre d'astéroides de la ligne
        List<Vector3> remaining_positions = new List<Vector3>(spawn_points);
        for (int i = 0; i < number_of_asteroids; i++)
        {
            //On choisit un lieu d'apparition de l'astéroide
            int position_number = Random.Range(0, remaining_positions.Count - 1); //Les positions dans la liste sont entre 0 et 5
            //On instancie l'obstacle (1/5 que ce soit un débris spatial indestructible)
            GameObject obstacle;
            if (Random.Range(0, 5f) >= 1f)
            {
                obstacle = Instantiate(asteroid, environment);
                obstacle.transform.localPosition = remaining_positions[position_number];
                obstacle.GetComponent<ChuteAsteroide>().SetBonusManager(bonus_manager);
                obstacle.transform.Rotate(new Vector3(0, 0, 1), Random.Range(0, 360f));
                //On détermine la vitesse de chute de l'astéroide
                obstacle.GetComponent<ChuteAsteroide>().falling_speed = Random.Range(max_fall_speed, min_fall_speed);
                //On détermine la vitesse et le sens de rotation de l'astéroide
                if (Random.value < 0.5)
                    obstacle.GetComponent<ChuteAsteroide>().rotation_speed = Random.Range(max_rotation_speed, min_rotation_speed);
                else
                    obstacle.GetComponent<ChuteAsteroide>().rotation_speed = -(Random.Range(max_rotation_speed, min_rotation_speed));
                //On active le script de déplacement de l'astéroide (après avoir déterminé les paramètres uniquement)
                obstacle.GetComponent<ChuteAsteroide>().enabled = true;
            }
            else
            {
                obstacle = Instantiate(ennemy_ship, environment);
                obstacle.transform.localPosition = remaining_positions[position_number];
                //On détermine la vitesse de chute du vaisseau
                obstacle.GetComponent<DeplacementVaisseau>().falling_speed = Random.Range(max_fall_speed, min_fall_speed);
                //On active le script de déplacement du vaisseau (après avoir déterminé les paramètres uniquement)
                obstacle.GetComponent<DeplacementVaisseau>().enabled = true;
            }
            
            remaining_positions.RemoveAt(position_number);
        }
    }
}
