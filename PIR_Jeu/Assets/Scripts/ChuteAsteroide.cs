using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuteAsteroide : MonoBehaviour
{
    public float falling_speed = 1;
    public float rotation_speed = 1;

    public GameObject piercing_bonus;

    private Transform transform;
    private Camera cam;

    private Material material;
    private bool dissolving = false;
    public float fade_per_second = 2;
    private float fade = 1;

    // Start is called before the first frame update
    private void Start()
    {
        material = GetComponent<Renderer>().material;
        transform = gameObject.transform;
        cam = Camera.main;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, 1), rotation_speed);
        transform.position += new Vector3(0, -(falling_speed * Time.fixedDeltaTime), 0);
        Vector3 position_in_camera = cam.WorldToViewportPoint(transform.position);
        if (dissolving)
        {
            fade -= fade_per_second * Time.fixedDeltaTime;
            material.SetFloat("_Fade", fade);
            if (fade < 0)
            {
                Destroy(gameObject);
            }
        }
        if (position_in_camera.y < -0.1)
        {
            Destroy(gameObject);
        }
    }

    public void Dissolve()
    {
        DisableColliders();
        dissolving = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<InfosVaisseau>().ReceiveDamage(1);
            DisableColliders();
            dissolving = true;
        }
    }

    private void DisableColliders()
    {
        BoxCollider2D[] colliders = gameObject.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D collider in colliders)
            collider.enabled = false;
    }

    private void OnDestroy()
    {
        if (Random.value < 0.1f)
        {
            Instantiate(piercing_bonus, transform.position, Quaternion.identity);
        }
    }
}
