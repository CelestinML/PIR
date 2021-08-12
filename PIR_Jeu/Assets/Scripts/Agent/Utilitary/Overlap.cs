using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Overlap : MonoBehaviour
{
    public float radius = 4f;

    public LayerMask layerMask;

    void Start()
    {
        transform.localScale = new Vector3(radius, radius, 0);
    }

    void Update()
    {
        Collider2D[] detected_collisions = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        int counter = 0;
        foreach (Collider2D collision in detected_collisions)
        {
            counter++;
        }

       Debug.Log(counter);
    }

    public List<GameObject> GetObstacles()
    {
        List<GameObject> obstacles = new List<GameObject>();
        Collider2D[] detected_collisions = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        foreach (Collider2D collision in detected_collisions)
        {
            obstacles.Add(collision.gameObject);
        }
        return obstacles;
    }
}
