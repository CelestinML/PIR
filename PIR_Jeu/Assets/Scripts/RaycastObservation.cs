using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastObservation : MonoBehaviour
{
    public float raycast_distance = 10f;

    public Transform raycast_left, raycast_center, raycast_right, raycast_top;

    public LayerMask layerMask;

    public bool draw_raycasts = true;

    public int nb_raycasts;
    public float min_angle, max_angle;

    private void Awake()
    {
        GameObject ship = transform.parent.parent.gameObject;
    }

    public List<Vector3> GetPositions()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        
        for (float angle = -85f; angle >= -90f; angle -= 5f)
        {
            hits.Add(Physics2D.Raycast(raycast_center.position, Quaternion.Euler(0, 0, angle) * Vector2.up, raycast_distance*(3f/4f), layerMask));
        }

        for (float angle = -80f; angle <= 80f; angle += 5f)
        {
            hits.Add(Physics2D.Raycast(raycast_center.position, Quaternion.Euler(0, 0, angle) * Vector2.up, raycast_distance, layerMask));
        }

        for (float angle = 85f; angle <= 90f; angle += 5f)
        {
            hits.Add(Physics2D.Raycast(raycast_center.position, Quaternion.Euler(0, 0, angle) * Vector2.up, raycast_distance * (3f / 4f), layerMask));
        }

        List<Vector3> distances = new List<Vector3>();
        //Debug.Log("Nb hits : " + hits.Count);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Asteroid")
                {
                    //Debug.Log("Asteroid detected : " + new Vector3(hit.collider.transform.localPosition.x, hit.collider.transform.localPosition.y, hit.collider.gameObject.GetComponent<ChuteAsteroide>().falling_speed));
                    distances.Add(new Vector3(hit.collider.transform.localPosition.x, hit.collider.transform.localPosition.y, hit.collider.gameObject.GetComponent<ChuteAsteroide>().falling_speed));
                }
                else if (hit.collider.tag == "Undestructible")
                {
                    //Debug.Log("Ship detected : " + new Vector3(hit.collider.transform.localPosition.x, hit.collider.transform.localPosition.y, hit.collider.gameObject.GetComponent<DeplacementVaisseau>().falling_speed));
                    distances.Add(new Vector3(hit.collider.transform.localPosition.x, hit.collider.transform.localPosition.y, hit.collider.gameObject.GetComponent<DeplacementVaisseau>().falling_speed));
                }
                else
                    distances.Add(new Vector3(-1000f, 1000f, 0f));
            }
            else
                distances.Add(new Vector3(-1000f, 1000f, 0f));
        }

        return distances;
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
            for (float angle = -85f; angle >= -90f; angle -= 5f)
            {
                Debug.DrawLine(raycast_center.position, raycast_top.position + Quaternion.Euler(0, 0, angle) * Vector2.up * raycast_distance*(3f/4f), Color.red);
            }

            for (float angle = -80f; angle <= 80f; angle += 5f)
            {
                Debug.DrawLine(raycast_center.position, raycast_top.position + Quaternion.Euler(0, 0, angle) * Vector2.up * raycast_distance, Color.red);
            }

            for (float angle = 85f; angle <= 90f; angle += 5f)
            {
                Debug.DrawLine(raycast_center.position, raycast_top.position + Quaternion.Euler(0, 0, angle) * Vector2.up * raycast_distance * (3f / 4f), Color.red);
            }
        }
    }
}
