using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastObservation : MonoBehaviour
{
    public float raycastDistance = 10f;

    public Transform raycast_left, raycast_center, raycast_right, raycast_top;

    public LayerMask layerMask;

    public bool draw_raycasts = true;

    public int nbRaycasts;
    private float angleIncr;
    public float minAngle, maxAngle;

    private void Awake()
    {
        GameObject ship = transform.parent.parent.gameObject;

        angleIncr = ((float)maxAngle - (float)minAngle) / ((float)nbRaycasts - 1.0f);
        Debug.Log("AngleIncr : " + angleIncr);
    }

    //The returned vectors will have the following informations :
    //- x contains the x position of the obstacle
    //- y contains the y position of the obstacle
    //- z contains the falling speed of the obstacle
    public List<Vector3> GetPositions()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        
        for (float angle = minAngle; angle <= maxAngle; angle += angleIncr)
        {
            Debug.Log("Launching at angle : " + angle);
            float distance = raycastDistance;
            //If the raycast is thrown towards left or right, it might interfer with other
            //environments. So we shorten the raycasts a little bit.
            if ((-100f < angle && angle < -80f) || (80f < angle && angle < 100f))
            {
                distance *= (2f / 3f);
            }
            hits.Add(Physics2D.Raycast(raycast_center.position, Quaternion.Euler(0, 0, angle) * Vector2.up, distance * (3f / 4f), layerMask));
        }

        List<Vector3> infos = new List<Vector3>();
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Asteroid")
                {
                    //Debug.Log("Asteroid detected : " + new Vector3(hit.collider.transform.localPosition.x, hit.collider.transform.localPosition.y, hit.collider.gameObject.GetComponent<ChuteAsteroide>().falling_speed));
                    infos.Add(new Vector3(hit.collider.transform.localPosition.x, hit.collider.transform.localPosition.y, hit.collider.gameObject.GetComponent<ChuteAsteroide>().falling_speed));
                }
                else if (hit.collider.tag == "Undestructible")
                {
                    //Debug.Log("Ship detected : " + new Vector3(hit.collider.transform.localPosition.x, hit.collider.transform.localPosition.y, hit.collider.gameObject.GetComponent<DeplacementVaisseau>().falling_speed));
                    infos.Add(new Vector3(hit.collider.transform.localPosition.x, hit.collider.transform.localPosition.y, hit.collider.gameObject.GetComponent<DeplacementVaisseau>().falling_speed));
                }
                else
                {
                    //We consider that the obstacle corresponding to the line is really far away
                    infos.Add(new Vector3(-1000f, 1000f, 0f));
                }
            }
            else
            {
                //We consider that the obstacle corresponding to the line is really far away
                infos.Add(new Vector3(-1000f, 1000f, 0f));
            }
        }

        return infos;
    }

    public List<float> GetDistances()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        hits.Add(Physics2D.Raycast(raycast_center.position, Vector2.up, 10f, layerMask)); // middle up shot

        hits.Add(Physics2D.Raycast(raycast_left.position, Vector2.left, 10f, layerMask)); // left left shot
        hits.Add(Physics2D.Raycast(raycast_right.position, Vector2.right, 10f, layerMask)); // right right shot

        hits.Add(Physics2D.Raycast(raycast_left.position, Vector2.up, 10f, layerMask)); // left up shot
        hits.Add(Physics2D.Raycast(raycast_right.position, Vector2.up, 10f, layerMask)); // right up shot


        hits.Add(Physics2D.Raycast(raycast_center.position, Quaternion.Euler(0, 0, -15) * Vector2.up, 10f, layerMask)); // 10 degrees middle left shot
        hits.Add(Physics2D.Raycast(raycast_center.position, Quaternion.Euler(0, 0, 15) * Vector2.up, 10f, layerMask)); // 10 degrees middle right shot

        hits.Add(Physics2D.Raycast(raycast_center.position, Quaternion.Euler(0, 0, -30) * Vector2.up, 10f, layerMask)); // 10 degrees middle left shot
        hits.Add(Physics2D.Raycast(raycast_center.position, Quaternion.Euler(0, 0, 30) * Vector2.up, 10f, layerMask));

        hits.Add(Physics2D.Raycast(raycast_center.position, Quaternion.Euler(0, 0, -40) * Vector2.up, 10f, layerMask)); // 10 degrees middle left shot
        hits.Add(Physics2D.Raycast(raycast_center.position, Quaternion.Euler(0, 0, 40) * Vector2.up, 10f, layerMask)); // 10 degrees middle right shot
        List<float> distances = new List<float>();
        //Debug.Log("Nb hits : " + hits.Count);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Asteroid")
                {
                    //Debug.Log("Asteroid detected : " + new Vector3(hit.collider.transform.localPosition.x, hit.collider.transform.localPosition.y, hit.collider.gameObject.GetComponent<ChuteAsteroide>().falling_speed));
                    distances.Add(hit.distance);
                }
                else if (hit.collider.tag == "Undestructible")
                {
                    //Debug.Log("Ship detected : " + new Vector3(hit.collider.transform.localPosition.x, hit.collider.transform.localPosition.y, hit.collider.gameObject.GetComponent<DeplacementVaisseau>().falling_speed));
                    distances.Add(hit.distance);
                }
                else
                    distances.Add(hit.distance);
            }
            else
                distances.Add(10f);
        }

        return distances;
    }

    private void Update()
    {
        
        if (draw_raycasts)
        {
            for (float angle = minAngle; angle <= maxAngle; angle += angleIncr)
            {
                float distance = raycastDistance;
                //If the raycast is thrown towards left or right, it might interfer with other
                //If the raycast is thrown towards left or right, it might interfer with other
                //environments. So we shorten the raycasts a little bit.
                if ((-100f < angle && angle < -80f) || (80f < angle && angle < 100f))
                {
                    distance *= (2f / 3f);
                }
                Debug.DrawLine(raycast_center.position, raycast_center.position + Quaternion.Euler(0, 0, angle) * Vector2.up * distance, Color.red);
            }
        }
    }
}
