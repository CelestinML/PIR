using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuteAsteroide : MonoBehaviour
{
    public float falling_speed = 1;
    public float rotation_speed = 1;

    public GameObject piercing_bonus;
    public GameObject shield_bonus;

    private Camera cam;

    private Material material;
    private bool dissolving = false;
    private float fade_per_second = 2;
    private float fade = 1;

    private Animator animator;

    private BonusManager bonus_manager;

    public Vector3 barrier;

    public void SetBonusManager(BonusManager bonus_manager)
    {
        this.bonus_manager = bonus_manager;
    }

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, 1), rotation_speed);
        transform.position += new Vector3(0, -(falling_speed * Time.fixedDeltaTime), 0);
        if (dissolving)
        {
            fade -= fade_per_second * Time.fixedDeltaTime;
            material.SetFloat("_Fade", fade);
            if (fade < 0)
            {
                SpawnBonus();
            }
        }
        if (transform.localPosition.y < barrier.y)
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
        if (collision.gameObject.tag == "Shield")
        {
            Dissolve();
        }
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInChildren<HealthManager>().ReceiveDamage(1);
            //DisableColliders();
            //animator.SetBool("Exploding", true);
        }
        
    }

    private void DisableColliders()
    {
        BoxCollider2D[] colliders = gameObject.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D collider in colliders)
            collider.enabled = false;
    }

    public void SpawnBonus()
    {
        bonus_manager.PopBonus(transform.localPosition);
        Destroy(gameObject);
    }
}
