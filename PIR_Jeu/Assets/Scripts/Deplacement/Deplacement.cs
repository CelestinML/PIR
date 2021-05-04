using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplacement : MonoBehaviour
{
    private CharacterController controller;
    private float input;

    private float last_input = 0;
    public float speed = 20;

    private Transform ship_transform;

    // Start is called before the first frame update
    void Start()
    {
        //We retrieve the ship's "position" (transform)
        ship_transform = transform.parent;
        input = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        input = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        //Debug.Log(input);
        //ship_transform.localPosition += new Vector3(1, 0, 0) * Time.fixedDeltaTime * speed * input;
        if (input > 0)
            MoveRight();
        else if (input < -0)
            MoveLeft();
        else
            DoNotMove();
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
}
