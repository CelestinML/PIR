using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsManager : MonoBehaviour
{
    public bool allow_weapons = true;

    private AudioSource audio_source;
    public GameObject piercing_image;
    public GameObject piercing_shot_ui;

    private string current_projectile = "basic";
    public GameObject basic_projectile;
    public GameObject piercing_projectile;

    public GameObject cooldown_sprite;
    public List<Sprite> cooldown_states;

    public float cooldown_time = 5;
    private bool can_shoot = true;
    private float time_since_last_shot = 0;
    public int max_number_of_piercing = 5;
    private int number_of_piercing_shot;

    public Transform projectile_barrier;

    private void Start()
    {
        audio_source = GameObject.FindWithTag("Sound").transform.GetChild(0).GetComponent<AudioSource>();

        if (!allow_weapons)
        {
            gameObject.SetActive(false);
        }

        if (piercing_image != null && piercing_shot_ui != null)
        {
            piercing_image.SetActive(false);
            piercing_shot_ui.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (can_shoot)
        {
            if (cooldown_sprite != null)
                cooldown_sprite.GetComponent<Image>().sprite = cooldown_states[cooldown_states.Count - 1];
            if (Input.GetButtonDown("Fire1"))
            {
                if (current_projectile == "basic")
                {
                    GameObject projectile = Instantiate(basic_projectile, gameObject.transform.position, Quaternion.identity);
                    projectile.GetComponent<ProjectileBehaviour>().projectile_barrier = projectile_barrier;
                }     
                    
                else if (current_projectile == "piercing")
                {
                    Debug.Log("Test");
                    number_of_piercing_shot++;
                    if (number_of_piercing_shot >= max_number_of_piercing)
                    {
                        if (piercing_image != null && piercing_shot_ui != null)
                        {
                            piercing_image.SetActive(false);
                            piercing_shot_ui.SetActive(false);
                        }
                        number_of_piercing_shot = 0;
                        current_projectile = "basic";
                    }
                    if (piercing_shot_ui != null)
                        piercing_shot_ui.GetComponent<TextMeshProUGUI>().text = "x " + (max_number_of_piercing - number_of_piercing_shot);
                    Instantiate(piercing_projectile, gameObject.transform.position, Quaternion.identity);
                }
                audio_source.Play(0);
                can_shoot = false;
            }
        }
        else
        {
            time_since_last_shot += Time.deltaTime;
            if (time_since_last_shot > cooldown_time)
            {
                can_shoot = true;
                time_since_last_shot = 0;
            }
            else
            {
                int i = Mathf.FloorToInt((time_since_last_shot / cooldown_time) * 9);
                if (cooldown_sprite != null)
                    cooldown_sprite.GetComponent<Image>().sprite = cooldown_states[i];
            }
        }
    }

    public void AllowPiercing()
    {
        if (piercing_image != null && piercing_shot_ui != null)
        {
            piercing_shot_ui.GetComponent<TextMeshProUGUI>().text = "x " + max_number_of_piercing;
            piercing_image.SetActive(true);
            piercing_shot_ui.SetActive(true);
        }
        current_projectile = "piercing";
        //Au cas où il y avait déjà un bonus de piercing
        number_of_piercing_shot = 0;
    }
}
