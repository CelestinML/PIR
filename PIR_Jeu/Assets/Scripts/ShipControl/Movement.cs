using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    private float input;

    private float last_input = 0;
    public float speed = 20;

    private Transform ship_transform;

    public List<Vector3> column_positions;

    public bool human_player = true;

    // Start is called before the first frame update
    void Awake()
    {
        //We retrieve the ship's "position" (transform)
        ship_transform = transform.parent;
        input = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        //Collecting the input if a human is playing
        input = Input.GetAxisRaw("Horizontal");
        //You should rather use this input method if you decided to use SetInput
        //input = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        /* If you don't use the SetInput method */
        if (human_player)
        {
            if (input > 0)
                MoveRight();
            else if (input < -0)
                MoveLeft();
            else
                DoNotMove();
        }
        /* If you do use the SetInput method */
        //ship_transform.localPosition += new Vector3(input * Time.fixedDeltaTime * speed, 0f, 0f);
    }

    //On définit chaque action possible clairement pour l'agent
    public void MoveLeft()
    {
        last_input = Mathf.Lerp(last_input, -1, 2 * Time.fixedDeltaTime);
        ship_transform.localPosition += new Vector3(last_input, 0, 0) * Time.fixedDeltaTime * speed;
    }

    public void MoveRight()
    {
        last_input = Mathf.Lerp(last_input, 1, 2 * Time.fixedDeltaTime);
        ship_transform.localPosition += new Vector3(last_input, 0, 0) * Time.fixedDeltaTime * speed;
    }

    public void DoNotMove()
    {
        last_input = Mathf.Lerp(last_input, 0, 5 * Time.fixedDeltaTime);
        ship_transform.localPosition += new Vector3(last_input, 0, 0) * Time.fixedDeltaTime * speed;
    }

    public void SetInput(float wanted_input)
    {
        input = wanted_input;
    }
}
