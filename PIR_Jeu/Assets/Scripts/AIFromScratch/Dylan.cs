using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dylan : MonoBehaviour
{
    public Deplacement deplacement;
    private float period = 0.2f;
    private float compteur = 0f;
    private int direction = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        compteur += Time.fixedDeltaTime;
        if (compteur > period)
        {
            compteur = 0f;
            direction = Random.Range(0, 3);
        }
        if (direction == 0)
        {
            deplacement.DoNotMove();
        }
        else if (direction == 1)
        {
            deplacement.MoveLeft();
        }
        else
        {
            deplacement.MoveRight();
        }
    }
}

