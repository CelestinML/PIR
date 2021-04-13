using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kevin
{
    private int[] shape;

    private float[][] neurons;
    private float[][][] weights;
    private float[][] biases;

    private float mutationProba = 1f;
    private float mutationIntensity = 0.2f;

    public float[][][] Weights { get => weights; set => weights = value; }
    public float[][] Biases { get => biases; set => biases = value; }
    public int[] Shape { get => shape; set => shape = value; }

    public Kevin(int inputsSize, int hiddenLayerSize, int outputsSize)
    {
        this.shape = new int[4] { inputsSize, hiddenLayerSize, hiddenLayerSize, outputsSize };

        InitNeurons();
        InitWeights();
        InitBiases();
    }

    public Kevin(int inputsSize, int hiddenLayerSize, int outputsSize, float[][][] pWeights, float[][] pBiases)
    {
        this.shape = new int[4] { inputsSize, hiddenLayerSize, hiddenLayerSize, outputsSize };
        this.weights = pWeights;
        this.biases = pBiases;

        InitNeurons();
    }


    // Neurons initialization
    private void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();
        for (int i = 0; i < shape.Length; i++)
        {
            neuronsList.Add(new float[shape[i]]);
        }
        neurons = neuronsList.ToArray();
    }


    /********************************************************
      NETWORK INITIALIZATION WITH Random WEIGHTS AND BIASES 
    *********************************************************/

    // Weights initialization
    private void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 1; i < shape.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = shape[i - 1];
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronWeights[k] = UnityEngine.Random.value - 0.5f;
                }
                layerWeightsList.Add(neuronWeights);
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        weights = weightsList.ToArray();
    }


    // Biases initialization
    private void InitBiases()
    {
        List<float[]> biasList = new List<float[]>();
        for (int i = 0; i < shape.Length; i++)
        {
            float[] bias = new float[shape[i]];
            for (int j = 0; j < shape[i]; j++)
            {
                bias[j] = UnityEngine.Random.value - 0.5f;
            }
            biasList.Add(bias);
        }
        biases = biasList.ToArray();
    }


    /**************************************************************
      NETWORK INITIALIZATION WITH PARAMETRIZED WEIGHTS AND BIASES 
    **************************************************************/

/*    private void LoadWeights(float[][][] pWeights)
    {
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 1; i < shape.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = shape[i - 1];
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronWeights[k] = pWeights[i][j][k];
                }
                layerWeightsList.Add(neuronWeights);
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        weights = weightsList.ToArray();
    }

    private void LoadBiases(float[][] pBiases)
    {
        List<float[]> biasList = new List<float[]>();
        for (int i = 0; i < shape.Length; i++)
        {
            float[] bias = new float[shape[i]];
            for (int j = 0; j < shape[i]; j++)
            {
                bias[j] = pBiases[i][j];
            }
            biasList.Add(bias);
        }
        biases = biasList.ToArray();
    }*/



    /********************
      FEEDFORWARD LOGIC
    ********************/

    private float Activate(float x)
    {
        return (float) System.Math.Tanh(x);
    }


    public float[] FeedForward(float[] pInputs)
    {
        float value;
        for (int i = 0; i < pInputs.Length; i++)
        {
            neurons[0][i] = pInputs[i];
        }
        for (int i = 1; i < shape.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                value = 0f;
                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = Activate(value + biases[i][j]);
            }
        }
        return neurons[shape.Length - 1];
    }

    public void Mutate()
    {
        MutateWeights();
        MutateBiases();
    }


    private void MutateWeights()
    {
        int compteur = 0;
        int total = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    total++;

                    if (UnityEngine.Random.value <= mutationProba)
                    {
                        compteur++;
                        float randomValue = UnityEngine.Random.value;
                        weights[i][j][k] = weights[i][j][k] + ((randomValue - 0.5f) * 2 * mutationIntensity);
                    }
                }
            }
        }
        //Debug.Log("Weights : On est rentré " + compteur + " fois dans la mutation sur un total de : " + total);
    }


    private void MutateBiases()
    {
        int compteur = 0;
        int total = 0;
        for (int i = 0; i < biases.Length; i++)
        {
            for (int j = 0; j < biases[i].Length; j++)
            {
                total++;
                if (UnityEngine.Random.value <= mutationProba)
                {
                    compteur++;
                    biases[i][j] = biases[i][j] + ((UnityEngine.Random.value - 0.5f) * 2 * mutationIntensity);
                }
            }
        }
        //Debug.Log("Biases : On est rentré " + compteur + " fois dans la mutation sur un total de : " + total);
    }

    public void PrintNetwork()
    {
        for(int i = 0; i < shape.Length; i++)
        {
            Debug.Log("COLUMN " + i + "of size " + shape[i]);
            for (int j = 0; j < shape[i]; j++)
            {
                Debug.Log("neuron " + j + " : " + neurons[i][j]);
            }
        }
    }


    override public String ToString()
    {
        String str = "";
        
        for(int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    str += (weights[i][j][k]).ToString();
                    str += " ";
                }
            }
        }

        str += "\n";

        for (int i = 0; i < biases.Length; i++)
        {
            for (int j = 0; j < biases[i].Length; j++)
            {
                str += (biases[i][j]).ToString();
                str += " ";
            }
        }

        return str;
    }
}
