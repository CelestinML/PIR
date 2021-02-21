using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    public bool shield_on = false;
    public GameObject shield;
    private float counter = 0;
    public float max_time = 8;
    private float start_blink_time;
    private float blink_period = 0.2f;
    private float blink_time = 0;
    // Start is called before the first frame update
    void Start()
    {
        start_blink_time = max_time * 70 / 100;
        shield.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (shield_on == true)
        {
            if (counter < max_time)
            {
                //if the shield is soon ending
                if (counter >= start_blink_time)
                {
                    blink_time += Time.fixedDeltaTime;
                    if (blink_time > blink_period)
                    {
                        if (shield.GetComponent<Renderer>().enabled)
                            shield.GetComponent<Renderer>().enabled = false;
                        else
                            shield.GetComponent<Renderer>().enabled = true;
                        blink_time = 0;
                    }
                }
                counter += Time.fixedDeltaTime;
            }
            else
            {
                shield.GetComponent<Renderer>().enabled = true;
                shield.SetActive(false);
                shield_on = false;
                counter = 0;
            }
        }
       
    }
    // Update is called once per frame
    public void Turn_On_Shield()
    {
        shield.SetActive(true);
        shield_on = true;
    }  
  
}
