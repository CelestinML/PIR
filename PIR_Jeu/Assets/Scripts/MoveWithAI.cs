using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MoveWithAI : MonoBehaviour
{
    public Movement deplacement;
    private float period = 0.1f;
    private float compteur = 0f;
    private int direction = 0;

    private Kevin kevin;

    private float[] inputs;
    private float[] outputs;

    public Transform raycastLeft, raycastMiddle, raycastRight; // Origins of raycasts
    public LayerMask layerMask; // Which is the collision layer for raycasts

    public RaycastObservation raycastObservation;

    private GenerationManager generationManager;

    private ShipSpawner shipSpawner;
    private int ship_index;

    private void Awake()
    {
        deplacement = transform.parent.GetComponentInChildren<Movement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        shipSpawner = transform.parent.gameObject.GetComponentInChildren<HealthManager>().ship_spawner;
        ship_index = shipSpawner.GetIndex(transform.parent.gameObject);

        generationManager = transform.parent.parent.gameObject.GetComponentInChildren<GenerationManager>();
        UpdateAI(generationManager.generation[ship_index]);

        inputs = new float[11];
        outputs = new float[3];
    }


    public void UpdateAI(Kevin pKevin)
    {
        kevin = pKevin;
    }

    public Kevin GetAI()
    {
        return kevin;
    }


    void FixedUpdate()
    {
        compteur += Time.fixedDeltaTime;

        if (compteur > period)
        {
            compteur = 0f;

            inputs = raycastObservation.GetDistances().ToArray();
            outputs = kevin.FeedForward(inputs);

            direction = GetMove();
        }

        if (direction == 0) deplacement.DoNotMove();
        if (direction == 1) deplacement.MoveLeft();
        if (direction == 2) deplacement.MoveRight();

    }


    private int GetMove()
    {
        List<float> tmpOutputs = new List<float>(outputs);
        return tmpOutputs.IndexOf(tmpOutputs.Max());
    }

    private void OnDestroy()
    {
        generationManager.NotifyDeath(ship_index);
    }

}

