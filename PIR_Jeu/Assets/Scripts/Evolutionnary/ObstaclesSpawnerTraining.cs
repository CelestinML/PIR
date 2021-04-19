using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObstaclesSpawnerTraining : MonoBehaviour
{
    public GameObject asteroide1;
    public GameObject vaisseau;

    public float min_fall_speed = 3;
    public float max_fall_speed = 3;

    public float min_rotation_speed = 1;
    public float max_rotation_speed = 4;

    private float time_since_last_spawn = 0;
    public float spawn_period = 2; //Temps en secondes

    private List<Vector3> spawn_points = new List<Vector3>();
    private List<List<bool>> asteroidsTemplate = new List<List<bool>>();
    private int templateIndex = 0;

    public bool loadTemplateFromFile = true;
    public string filename = "level1.lvl";

    // Start is called before the first frame update
    void Start()
    {
        spawn_points.Add(new Vector3(-3.61f, 5.75f, 0));
        spawn_points.Add(new Vector3(-2.18f, 5.75f, 0));
        spawn_points.Add(new Vector3(-0.78f, 5.75f, 0));
        spawn_points.Add(new Vector3(0.58f, 5.75f, 0));
        spawn_points.Add(new Vector3(2.03f, 5.75f, 0));
        spawn_points.Add(new Vector3(3.49f, 5.75f, 0));

        if(loadTemplateFromFile)
        {
            LoadAsteroidsTemplate(filename);
        }
    }

    private void FixedUpdate()
    {
        time_since_last_spawn += Time.fixedDeltaTime;
        if (time_since_last_spawn > spawn_period)
        {
            if(loadTemplateFromFile)
            {
                SpawnAsteroidsFromTemplate();
            }
            else
            {
                SpawnAsteroids();
            }
            time_since_last_spawn = 0;
        }
    }


    public void Reset()
    {
        if(loadTemplateFromFile)
        {
            templateIndex = 0;
        }
    }


    private void LoadAsteroidsTemplate(string filename)
    {
        string path = Directory.GetCurrentDirectory() + "/" + filename;

        if(File.Exists(path))
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach(string line in lines)
            {
                List<bool> row = new List<bool>();
                string[] lineSplited = line.Split(' ');
                foreach(string charac in lineSplited)
                {
                    if (charac == "X")
                    {
                        row.Add(true);
                    }
                    else if (charac == "O")
                    {
                        row.Add(false);
                    }
                    else if (charac == "\n" || charac == "") { }
                    else
                    {
                        Debug.Log("Unvalid character please check your txt file");
                    }
                }
                asteroidsTemplate.Add(row);
            }
        }
        else
        {
            Debug.Log("No such file, be sure to spell the filename correctly with the extension and move it to the root of the project");
        }
    }


    private void SpawnAsteroidsFromTemplate()
    {
        for (int i = 0; i < 6; i++)
        {
            if (asteroidsTemplate[templateIndex][i])
            {
                GameObject obstacle;

                obstacle = Instantiate(asteroide1, spawn_points[i], Quaternion.identity);
                obstacle.transform.Rotate(new Vector3(0, 0, 1), Random.value * 360f);
                //On détermine la vitesse de chute de l'astéroide
                obstacle.GetComponent<ChuteAsteroideTraining>().falling_speed = min_fall_speed + Random.value * (max_fall_speed - min_fall_speed);
                //On détermine la vitesse et le sens de rotation de l'astéroide
                if (Random.value < 0.5)
                    obstacle.GetComponent<ChuteAsteroideTraining>().rotation_speed = min_rotation_speed + Random.value * (max_rotation_speed - min_rotation_speed);
                else
                    obstacle.GetComponent<ChuteAsteroideTraining>().rotation_speed = -(min_rotation_speed + Random.value * (max_rotation_speed - min_rotation_speed));
                //On active le script de déplacement de l'astéroide (après avoir déterminé les paramètres uniquement)
                obstacle.GetComponent<ChuteAsteroideTraining>().enabled = true;
            }
        }
        if(templateIndex >= asteroidsTemplate.Count)
        {
            templateIndex = 0;
        }
        else
        {
            templateIndex++;
        }
    }


    private void SpawnAsteroids()
    {
        //On fait apparaître entre 1 et 5 astéroides par ligne
        int number_of_asteroids = 1 + (int)(Random.value * 4f); //A chaque spawn d'asteroides, on détermine aléatoirement le nombre d'astéroides de la ligne
        List<Vector3> remaining_positions = new List<Vector3>(spawn_points);
        for (int i = 0; i < number_of_asteroids; i++)
        {
            //On choisit un lieu d'apparition de l'astéroide
            int position_number = (int)Mathf.Floor(Random.value * remaining_positions.Count); //Les positions dans la liste sont entre 0 et 5
            //On instancie l'obstacle (1/5 que ce soit un débris spatial indestructible)
            GameObject obstacle;
            if (Random.value >= 1.0f / 5.0f)
            {
                obstacle = Instantiate(asteroide1, remaining_positions[position_number], Quaternion.identity);
                obstacle.transform.Rotate(new Vector3(0, 0, 1), Random.value * 360f);
                //On détermine la vitesse de chute de l'astéroide
                obstacle.GetComponent<ChuteAsteroideTraining>().falling_speed = min_fall_speed + Random.value * (max_fall_speed - min_fall_speed);
                //On détermine la vitesse et le sens de rotation de l'astéroide
                if (Random.value < 0.5)
                    obstacle.GetComponent<ChuteAsteroideTraining>().rotation_speed = min_rotation_speed + Random.value * (max_rotation_speed - min_rotation_speed);
                else
                    obstacle.GetComponent<ChuteAsteroideTraining>().rotation_speed = -(min_rotation_speed + Random.value * (max_rotation_speed - min_rotation_speed));
                //On active le script de déplacement de l'astéroide (après avoir déterminé les paramètres uniquement)
                obstacle.GetComponent<ChuteAsteroideTraining>().enabled = true;
            }
            else
            {
                obstacle = Instantiate(vaisseau, remaining_positions[position_number], Quaternion.identity);
                //On détermine la vitesse de chute du vaisseau
                obstacle.GetComponent<DeplacementVaisseauTraining>().falling_speed = min_fall_speed + Random.value * (max_fall_speed - min_fall_speed);
                //On active le script de déplacement du vaisseau (après avoir déterminé les paramètres uniquement)
                obstacle.GetComponent<DeplacementVaisseauTraining>().enabled = true;
            }
            
            remaining_positions.RemoveAt(position_number);
        }
    }
}
