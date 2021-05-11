using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateUpdateGeneration : MonoBehaviour
{
    public GameObject generationManager;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("Generation").Length == 0)
        {
            Instantiate(generationManager, transform.parent.parent);
        }
    }
}
