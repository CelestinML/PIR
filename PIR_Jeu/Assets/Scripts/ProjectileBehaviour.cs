using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    public float speed = 20;

    private Transform transform;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.transform;
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0, speed * Time.fixedDeltaTime, 0);
        Vector3 position_in_camera = cam.WorldToViewportPoint(transform.position);
        if (position_in_camera.y > 1.1)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Undestructible")
        {
            Destroy(gameObject);
        }
    }
}
