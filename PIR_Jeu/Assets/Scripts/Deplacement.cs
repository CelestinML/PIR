using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplacement : MonoBehaviour
{
    private CharacterController controller;
    private Transform transform;
    private float input;

    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.transform;
        input = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        input = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(1, 0, 0) * Time.fixedDeltaTime * speed * input;
    }
}
