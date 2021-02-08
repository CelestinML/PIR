using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplacement : MonoBehaviour
{
    private CharacterController controller;
    private Transform transform;
    private float input;

    private float last_input = 0;
    public float speed = 20;

    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.transform;
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
        //transform.position += new Vector3(1, 0, 0) * Time.fixedDeltaTime * speed * input;
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
        transform.position += new Vector3(last_input, 0, 0) * Time.fixedDeltaTime * speed;
    }

    public void MoveRight()
    {
        last_input = Mathf.Lerp(last_input, 1, 2 * Time.fixedDeltaTime);
        transform.position += new Vector3(last_input, 0, 0) * Time.fixedDeltaTime * speed;
    }

    public void DoNotMove()
    {
        last_input = Mathf.Lerp(last_input, 0, 5 * Time.fixedDeltaTime);
        transform.position += new Vector3(last_input, 0, 0) * Time.fixedDeltaTime * speed;
    }
}
