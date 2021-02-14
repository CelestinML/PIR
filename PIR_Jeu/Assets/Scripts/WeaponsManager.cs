using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsManager : MonoBehaviour
{
    public AudioSource audio;

    public string current_projectile = "basic";
    public GameObject basic_projectile;
    public GameObject piercing_projectile;

    public GameObject cooldown_sprite;
    public Sprite cooldown_1;
    public Sprite cooldown_2;
    public Sprite cooldown_3;
    public Sprite cooldown_4;
    public Sprite cooldown_5;
    public Sprite cooldown_6;
    public Sprite cooldown_7;
    public Sprite cooldown_8;
    public Sprite cooldown_9;
    public Sprite cooldown_10;
    private List<Sprite> cooldown_states;

    public float cooldown = 5;
    private bool can_shoot = true;
    private float time_since_last_shot = 0;

    private void Start()
    {
        cooldown_states = new List<Sprite>();
        cooldown_states.Add(cooldown_1);
        cooldown_states.Add(cooldown_2);
        cooldown_states.Add(cooldown_3);
        cooldown_states.Add(cooldown_4);
        cooldown_states.Add(cooldown_5);
        cooldown_states.Add(cooldown_6);
        cooldown_states.Add(cooldown_7);
        cooldown_states.Add(cooldown_8);
        cooldown_states.Add(cooldown_9);
        cooldown_states.Add(cooldown_10);
    }

    // Update is called once per frame
    void Update()
    {
        if (can_shoot)
        {
            cooldown_sprite.GetComponent<Image>().sprite = cooldown_states[9];
            if (Input.GetButtonDown("Fire1"))
            {
                if(current_projectile == "basic")
                    Instantiate(basic_projectile, gameObject.transform.position, Quaternion.identity);
                else if (current_projectile == "piercing")
                    Instantiate(piercing_projectile, gameObject.transform.position, Quaternion.identity);
                audio.Play(0);
                can_shoot = false;
            }
        }
        else
        {
            time_since_last_shot += Time.deltaTime;
            if (time_since_last_shot > cooldown)
            {
                can_shoot = true;
                time_since_last_shot = 0;
            }
            else
            {
                int i = Mathf.FloorToInt((time_since_last_shot / cooldown) * 9);
                Debug.Log(i);
                cooldown_sprite.GetComponent<Image>().sprite = cooldown_states[i];
            }
        }
    }
}
