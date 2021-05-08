using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    public float speed = 20;
    public bool piercing = false;

    public GameObject vaisseau;

    private Camera cam;

    public Transform projectile_barrier;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0, speed * Time.fixedDeltaTime, 0);
        if (transform.localPosition.y > projectile_barrier.position.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            collision.gameObject.GetComponent<ChuteAsteroide>().Dissolve();
            if (!piercing)
                Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Undestructible")
        {
            Destroy(gameObject);
        }
    }
}
