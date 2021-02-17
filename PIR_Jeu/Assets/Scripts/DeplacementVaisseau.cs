using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementVaisseau : MonoBehaviour
{
    public float falling_speed = 1;
    //public float rotation_speed = 1;

    private Camera cam;

    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
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
            collision.gameObject.GetComponent<InfosVaisseau>().ReceiveDamage(1);
            DisableColliders();
            //Lancer une animation de destruction du vaisseau ?
        }
    }

    private void DisableColliders()
    {
        BoxCollider2D[] colliders = gameObject.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D collider in colliders)
            collider.enabled = false;
    }
}
