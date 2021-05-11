using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMovementScript : MonoBehaviour
{
    void Awake()
    {
        GetComponent<AgentReinforcement>().movement = transform.parent.GetComponentInChildren<Movement>();
    }
}
