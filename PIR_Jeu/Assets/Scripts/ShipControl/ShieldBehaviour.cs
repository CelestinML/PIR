using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            collision.gameObject.GetComponent<ChuteAsteroide>().Dissolve();
        }
        if (collision.gameObject.tag == "Undestructible")
        {
            collision.gameObject.GetComponent<DeplacementVaisseau>().Dissolve() ;
        }
    }
}
