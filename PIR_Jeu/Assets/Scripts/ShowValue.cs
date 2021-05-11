using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowValue : MonoBehaviour
{
    public Text text;

    public void UpdateValue(float value)
    {
        text.text = Mathf.Round(value).ToString();
    }
}
