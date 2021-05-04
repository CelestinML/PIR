using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    public void HandleDeath()
    {
        Destroy(gameObject);
    }
}
