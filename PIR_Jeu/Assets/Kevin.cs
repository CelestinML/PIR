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

    public int inputLayerSize = 11;
    public int hiddenLayerSize = 20;
    public int outputLayerSize = 3;
    public int numberOfHiddenLayers = 2;


    // Create a new neural network with #inputLayerSize inputs neurons,
    // #hiddenLayerSize hidden layers of #numberOfHiddenLayers neurons,
    // #outputLayerSize outputs neurons.
    // The wieghts and biases are initialized from pWeights and pBiases.
    // The mutation intensity and proba are set to pMutationProba and pMutationIntensity.
    public Kevin(float[][][] pWeights, float[][] pBiases, float pMutationProba, float pMutationIntensity)
    {
        List<int> shapeList = new List<int>();
        shapeList.Add(inputLayerSize);
        for (int i = 0; i < numberOfHiddenLayers; ++i)
        {
            shapeList.Add(hiddenLayerSize);
        }
        shapeList.Add(outputLayerSize);

        this.shape = shapeList.ToArray();
        this.Weights = pWeights;
        this.Biases = pBiases;

        mutationProba = pMutationProba;
        mutationIntensity = pMutationIntensity;

        InitNeurons();
    }



    // Create a new neural network with inputsSize inputs neurons, 2 hidden layers of hiddenLayerSize neurons, outputsSize outputs neurons.
    // The wieghts and biases are randomly initialized.
    // The mutation intensity and proba are set to pMutationProba and pMutationIntensity.
    public Kevin(int inputsSize, int hiddenSize, int outputsSize, float pMutationProba, float pMutationIntensity)
    {
        List<int> shapeList = new List<int>();
        shapeList.Add(inputsSize);
        for (int i = 0; i < numberOfHiddenLayers; ++i)
        {
            shapeList.Add(hiddenSize);
        }
        shapeList.Add(outputsSize);

        this.shape = shapeList.ToArray();

        mutationProba = pMutationProba;
        mutationIntensity = pMutationIntensity;

        InitNeurons();
        InitWeights();
        InitBiases();
    }


    // Create a new neural network with inputsSize inputs neurons, 2 hidden layers of hiddenLayerSize neurons, outputsSize outputs neurons
    // The weights and biases are initialized from pFlatWeights and pFlatBiases.
    // The mutation intensity and proba are set to pMutationProba and pMutationIntensity.
    public Kevin(int inputsSize, int hiddenSize, int outputsSize, float[] pFlatWeights, float[] pFlatBiases, float pMutationProba, float pMutationIntensity)
    {
        List<int> shapeList = new List<int>();
        shapeList.Add(inputsSize);
        for (int i = 0; i < numberOfHiddenLayers; ++i)
        {
            shapeList.Add(hiddenSize);
        }
        shapeList.Add(outputsSize);

        this.shape = shapeList.ToArray();

        mutationProba = pMutationProba;
        mutationIntensity = pMutationIntensity;

        InitNeurons();

        InitWeightsWithParams(pFlatWeights);
        InitBiasesWithParams(pFlatBiases);
    }


    // Create a new neural network with inputsSize inputs neurons, 2 hidden layers of hiddenLayerSize neurons, outputsSize outputs neurons
    // The wieghts and biases are initialized from pWeights and pBiases.
    // The mutation intensity and proba are set to pMutationProba and pMutationIntensity.




    /********************************************************
      NETWORK INITIALIZATION WITH Random WEIGHTS AND BIASES 
    *********************************************************/

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




    /********************
      FEEDFORWARD LOGIC
    ********************/

    private float Activate(float x)
    {
        return (float)Math.Tanh(x);
    }

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





    /**********************
      MUTATION OPERATIONS
     *********************/


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
                        float randomValue = UnityEngine.Random.Range(-mutationIntensity, mutationIntensity);
                        Weights[i][j][k] = Weights[i][j][k] + randomValue;
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
                    float randomValue = UnityEngine.Random.Range(-mutationIntensity, mutationIntensity);
                    Biases[i][j] = Biases[i][j] + randomValue;
                }
            }
        }
    }







    /********************************
      UTILITARY AND DEBUG FUNCTIONS
    *********************************/

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