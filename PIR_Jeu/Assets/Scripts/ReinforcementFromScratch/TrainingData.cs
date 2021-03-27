using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingData
{
    private float[] state;
    private float qValue;
    private float reward;

    public TrainingData(float[] stateInit, float qValueInit, float rewardInit)
    {
        state = stateInit;
        qValue = qValueInit;
        reward = rewardInit;
    }

    public float[] State { get => state; set => state = value; }
    public float QValue { get => qValue; set => qValue = value; }
    public float Reward { get => reward; set => reward = value; }

    public override string ToString()
    {
        string str = "";
        foreach(float item in state)
        { 
            str += item.ToString() + " ";
        }

        str += qValue.ToString() + " ";
        str += reward.ToString() + "\n";
        return str;
    }
}
