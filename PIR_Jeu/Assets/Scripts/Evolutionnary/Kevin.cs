using System;
using System.Collections.Generic;
using UnityEngine;

public class Kevin
{
    private int[] shape;

    private float[][] neurons;
    private float mutationProba;
    private float mutationIntensity;

    public float[][][] Weights { get; set; }
    public float[][] Biases { get; set; }



    // Create a new neural network with 7 inputs neurons, 2 hidden layers of 8 neurons, 6 outputs neurons.
    // The wieghts and biases are randomly initialized.
    // The mutation intensity and proba are to their default value : 0.1f.
    public Kevin()
    {
        this.shape = new int[4] { 9, 8, 8, 3 };

        mutationProba = 0.1f;
        mutationIntensity = 0.1f;

        InitNeurons();
        InitWeights();
        InitBiases();
    }


    // Create a new neural network with 7 inputs neurons, 2 hidden layers of 8 neurons, 3 outputs neurons.
    // The wieghts and biases are initialized from pWeights and pBiases.
    // The mutation intensity and proba are set to pMutationProba and pMutationIntensity.
    public Kevin(float[][][] pWeights, float[][] pBiases, float pMutationProba, float pMutationIntensity)
    {
        this.shape = new int[4] { 9, 8, 8, 3 };
        this.Weights = pWeights;
        this.Biases = pBiases;

        mutationProba = pMutationProba;
        mutationIntensity = pMutationIntensity;

        InitNeurons();
    }


    // Create a new neural network with inputsSize inputs neurons, 2 hidden layers of hiddenLayerSize neurons, outputsSize outputs neurons.
    // The wieghts and biases are randomly initialized.
    // The mutation intensity and proba are to their default value : 0.1f.
    public Kevin(int inputsSize, int hiddenLayerSize, int outputsSize)
    {
        this.shape = new int[4] { inputsSize, hiddenLayerSize, hiddenLayerSize, outputsSize };

        mutationProba = 0.1f;
        mutationIntensity = 0.1f;

        InitNeurons();
        InitWeights();
        InitBiases();
    }


    // Create a new neural network with inputsSize inputs neurons, 2 hidden layers of hiddenLayerSize neurons, outputsSize outputs neurons.
    // The wieghts and biases are randomly initialized.
    // The mutation intensity and proba are set to pMutationProba and pMutationIntensity.
    public Kevin(int inputsSize, int hiddenLayerSize, int outputsSize, float pMutationProba, float pMutationIntensity)
    {
        this.shape = new int[4] { inputsSize, hiddenLayerSize, hiddenLayerSize, outputsSize };

        mutationProba = pMutationProba;
        mutationIntensity = pMutationIntensity;

        InitNeurons();
        InitWeights();
        InitBiases();
    }


    // Create a new neural network with inputsSize inputs neurons, 2 hidden layers of hiddenLayerSize neurons, outputsSize outputs neurons
    // The wieghts and biases are initialized from pFlatWeights and pFlatBiases.
    // The mutation intensity and proba are set to pMutationProba and pMutationIntensity.
    public Kevin(int inputsSize, int hiddenLayerSize, int outputsSize, float[] pFlatWeights, float[] pFlatBiases, float pMutationProba, float pMutationIntensity)
    {
        this.shape = new int[4] { inputsSize, hiddenLayerSize, hiddenLayerSize, outputsSize };

        mutationProba = pMutationProba;
        mutationIntensity = pMutationIntensity;

        InitNeurons();

        InitWeightsWithParams(pFlatWeights);
        InitBiasesWithParams(pFlatBiases);
    }


    // Create a new neural network with inputsSize inputs neurons, 2 hidden layers of hiddenLayerSize neurons, outputsSize outputs neurons
    // The wieghts and biases are initialized from pWeights and pBiases.
    // The mutation intensity and proba are set to pMutationProba and pMutationIntensity.
    public Kevin(int inputsSize, int hiddenLayerSize, int outputsSize, float[][][] pWeights, float[][] pBiases, float pMutationProba, float pMutationIntensity)
    {
        this.shape = new int[4] { inputsSize, hiddenLayerSize, hiddenLayerSize, outputsSize };
        this.Weights = pWeights;
        this.Biases = pBiases;

        mutationProba = pMutationProba;
        mutationIntensity = pMutationIntensity;

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
        Weights = weightsList.ToArray();
    }

    private void InitWeightsWithParams(float[] pWeights)
    {
        List<float[][]> weightsList = new List<float[][]>();
        int cpt = 0; 

        for (int i = 1; i < shape.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = shape[i - 1];
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    Debug.Log(pWeights[cpt]);
                    neuronWeights[k] = pWeights[cpt];
                    Debug.Log(neuronWeights[k]);
                    cpt++;
                }
                layerWeightsList.Add(neuronWeights);
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        Weights = weightsList.ToArray();
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
        Biases = biasList.ToArray();
    }


    private void InitBiasesWithParams(float[] pBiases)
    {
        List<float[]> biasList = new List<float[]>();
        int cpt = 0;

        for (int i = 0; i < shape.Length; i++)
        {
            float[] bias = new float[shape[i]];
            for (int j = 0; j < shape[i]; j++)
            {
                bias[j] = pBiases[cpt];
                cpt++;
            }
            biasList.Add(bias);
        }
        Biases = biasList.ToArray();
    }


    private float Activate(float x)
    {
        // return (float) Math.Tanh(x);
        return x;
    }


    /********************
      FEEDFORWARD LOGIC
    ********************/
    public float[] FeedForward(float[] pInputs)
    {
        float value;
        for (int i = 0; i < pInputs.Length; i++)
        {
            neurons[0][i] = pInputs[i];
        }
        for (int i = 1; i < shape.Length; i++) // for each layer
        {
            for (int j = 0; j < neurons[i].Length; j++) // for each neuron in layer
            {
                value = 0f;
                for (int k = 0; k < neurons[i - 1].Length; k++) // for each neurons in previous layer
                {
                    value += Weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = Activate(value + Biases[i][j]);
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
        for (int i = 0; i < Weights.Length; i++)
        {
            for (int j = 0; j < Weights[i].Length; j++)
            {
                for (int k = 0; k < Weights[i][j].Length; k++)
                {
                    if (UnityEngine.Random.value <= mutationProba)
                    {
                        float randomValue = UnityEngine.Random.value;
                        Weights[i][j][k] = Weights[i][j][k] + ((randomValue - 0.5f) * 2 * mutationIntensity);
                    }
                }
            }
        }
    }


    private void MutateBiases()
    {
        for (int i = 0; i < Biases.Length; i++)
        {
            for (int j = 0; j < Biases[i].Length; j++)
            {
                if (UnityEngine.Random.value <= mutationProba)
                {
                    Biases[i][j] = Biases[i][j] + ((UnityEngine.Random.value - 0.5f) * 2 * mutationIntensity);
                }
            }
        }
    }

    public void PrintNetwork()
    {
        for (int i = 0; i < shape.Length; i++)
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

        for (int i = 0; i < Weights.Length; i++)
        {
            for (int j = 0; j < Weights[i].Length; j++)
            {
                for (int k = 0; k < Weights[i][j].Length; k++)
                {
                    str += (Weights[i][j][k]).ToString();
                    str += " ";
                }
            }
        }

        str += "\n";

        for (int i = 0; i < Biases.Length; i++)
        {
            for (int j = 0; j < Biases[i].Length; j++)
            {
                str += (Biases[i][j]).ToString();
                str += " ";
            }
        }

        return str;
    }
}