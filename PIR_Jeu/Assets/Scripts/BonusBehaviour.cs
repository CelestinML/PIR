using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBehaviour : MonoBehaviour
{
    public string bonus_type;
    public string projectile_name;

    public float falling_speed = 3;

    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(0, -falling_speed * Time.fixedDeltaTime, 0);

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
            if (bonus_type == "Projecile")
            {
                collision.gameObject.GetComponent<WeaponsManager>().current_projectile = projectile_name;
            }
            else if (bonus_type == "Shield")
            {
                collision.gameObject.GetComponent<ShieldManager>().Turn_On_Shield();
            }
            
            Destroy(gameObject);
        }
    }
}
