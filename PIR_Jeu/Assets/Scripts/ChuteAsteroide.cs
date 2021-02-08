using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuteAsteroide : MonoBehaviour
{
    public float falling_speed = 1;
    public float rotation_speed = 1;

    private Transform transform;
    private Camera cam;

    // Start is called before the first frame update
    private void Start()
    {
        transform = gameObject.transform;
        cam = Camera.main;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, 1), rotation_speed);
        transform.position += new Vector3(0, -(falling_speed * Time.fixedDeltaTime), 0);
        Vector3 position_in_camera = cam.WorldToViewportPoint(transform.position);
        if (position_in_camera.y < -0.1)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SetActive(false);
            Camera.main.GetComponent<ObstaclesSpawner>().enabled = false;
        }
    }
}
