using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public List<GameObject> bonus_prefabs;

    public bool activate_bonuses = true;

    private Transform environment;

    private void Start()
    {
        environment = transform.parent;
    }

    public void PopBonus(Vector3 position)
    {
        if (activate_bonuses)
        {
            if (Random.value < 0.1f)
            {
                GameObject bonus = Instantiate(bonus_prefabs[Random.Range(0, bonus_prefabs.Count)], environment);
                bonus.transform.localPosition = position;
            }
        }
    }
}
