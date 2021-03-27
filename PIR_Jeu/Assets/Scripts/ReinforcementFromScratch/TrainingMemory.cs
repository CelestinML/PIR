using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingMemory : MonoBehaviour
{
    List<TrainingData> list_Data = new List<TrainingData>();

    public void append_Data(TrainingData data)
    {
        list_Data.Add(data);
    }

    public TrainingData get_Data(int index)
    {
        TrainingData[] array_Data = list_Data.ToArray();
        return array_Data[index];
    }
}
