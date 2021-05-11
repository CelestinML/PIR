using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuAI : MonoBehaviour
{
    public static int nb_environments;
    public static int nb_ships;
    public static bool allow_weapons;
    public static string ai;

    private TMPro.TMP_Dropdown weapon_dropdown;
    private TMPro.TMP_Dropdown ai_type_dropdown;

    public void Start()
    {
        mainMenu.human_player = false;
        weapon_dropdown = GameObject.Find("Weapon_Dropdown").GetComponent<TMPro.TMP_Dropdown>();
        ai_type_dropdown = GameObject.Find("AI_Type_Dropdown").GetComponent<TMPro.TMP_Dropdown>();
    }
    public void Update()
    {
        nb_environments = int.Parse(GameObject.Find("Nb_Environments_Value").GetComponent<Text>().text);
        nb_ships = int.Parse(GameObject.Find("Nb_Ships_Value").GetComponent<Text>().text);
        SelectWeaponValue();
        SelectAIValue();
    }
    public void LaunchGame()
    {
        SceneManager.LoadScene("HumanPlayerScene");
        
        /*
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
            objectsInScene.Add(go);
        }


        Debug.Log(objectsInScene.Count);
        for (int i = 0; i < objectsInScene.Count; i++)
        {
            Debug.Log(objectsInScene[i].name);
            if (objectsInScene[i].name == "EnvironmentSpawner")
            {
                Debug.Log("OK");
                GameObject environmentSpawner = objectsInScene[i];
                environmentSpawner.GetComponent<EnvironmentSpawner>().nb_environments = 3;
                environmentSpawner.SetActive(true);
                break;
            }
        }*/
    }

    public void SelectWeaponValue()
    {
        int value = weapon_dropdown.value;
        string option_value = weapon_dropdown.options[value].text;
        if (option_value == "Yes")
        {
            allow_weapons = true;
        }
        else
        {
            allow_weapons = false;
        }
    }

    public void SelectAIValue()
    {
        int value = ai_type_dropdown.value;
        string option_value = ai_type_dropdown.options[value].text;
        ai = option_value;
    }
}
