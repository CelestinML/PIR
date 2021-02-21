using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementVaisseau : MonoBehaviour
{
    public float falling_speed = 1;
    //public float rotation_speed = 1;
    private Material material;
    private bool dissolving = false;
    public float fade_per_second = 2;
    private float fade = 1;

    private Camera cam;

    // Start is called before the first frame update
    private void Start()
    {
        material = GetComponent<Renderer>().material;
        cam = Camera.main;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += new Vector3(0, -(falling_speed * Time.fixedDeltaTime), 0);
        Vector3 position_in_camera = cam.WorldToViewportPoint(transform.position);
        if (dissolving)
        {
            fade -= fade_per_second * Time.fixedDeltaTime;
            material.SetFloat("_Fade", fade);      
        }
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

    public void Dissolve()
    {
        DisableColliders();
        dissolving = true;
    }

    private void DisableColliders()
    {
        EdgeCollider2D[] colliders = gameObject.GetComponents<EdgeCollider2D>();
        foreach (EdgeCollider2D collider in colliders)
            collider.enabled = false;
    }
}
