using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBehaviour : MonoBehaviour
{
    public string bonus_type;
    public string projectile_name;

    public Transform barrier;

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
        if (transform.localPosition.y < barrier.localPosition.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (bonus_type == "projectile")
            {
                if (projectile_name == "piercing")
                {
                    Debug.Log("Allowing piercing projectile");
                    collision.transform.GetChild(1).gameObject.GetComponent<WeaponsManager>().AllowPiercing();
                }
            }
            else if (bonus_type == "shield")
            {
                collision.transform.GetChild(0).GetComponent<ShieldManager>().TurnOnShield();
            }
            
            Destroy(gameObject);
        }
    }
}
