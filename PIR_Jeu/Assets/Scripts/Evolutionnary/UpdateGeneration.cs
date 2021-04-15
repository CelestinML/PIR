using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UpdateGeneration
{
    private bool loadFromFile = false;

    private int nbChildren = 35;
    private int nbBest = 6;
    private int nbChildrenPerBest = 5;
    private int nbRandom = 5;

    public MoveWithAI moveHandler;

    private Kevin[] children;
    private float[] scores;

    private Kevin[] bests;


    public UpdateGeneration()
    {
        children = new Kevin[nbChildren];
        scores = new float[nbChildren];

        bests = new Kevin[nbBest];
    }


    public void CreateGeneration()
    {
        if(loadFromFile)
        {
            GetBestsFromFile();
            SelectGeneration();
        }
        else
        {
            children = new Kevin[nbChildren];

            for (int i = 0; i < nbChildren; i++)
            {
                children[i] = new Kevin(7, 8, 3);
                Debug.Log("Kevin n" + i + " has been created");
            }
        }
    }


    private void SelectGeneration()
    {
        Kevin[] tmp_Children = new Kevin[nbChildren];
        int cpt = 0;

        for (int i = 0; i < nbBest; i++)
        {
            for(int j = 0; j < nbChildrenPerBest; j++)
            {
                // Kevin tmp = new Kevin();     
                Kevin tmp = new Kevin(Copy(bests[i].Weights), bests[i].Biases);
                tmp.Mutate();
                children[cpt] = tmp;
                cpt++;
            }
        }

        for (int i = 0; i < nbRandom; i++)
        {
            children[cpt] = new Kevin(7, 8, 3);
        }

        for (int i = 0; i < tmp_Children.Length; i++)
        {
            Debug.Log("Children[" + i + "] = " + children[i]);
        }
    }

    private static float[][][] Copy(float[][][] source)
    {
        float[][][] dest = new float[source.Length][][];
        for (int x = 0; x < source.Length; x++)
        {
            float[][] s = new float[source[x].Length][];
            for (int y = 0; y < source[x].Length; y++)
            {
                float[] n = new float[source[x][y].Length];
                int length = source[x][y].Length * sizeof(int);
                Buffer.BlockCopy(source[x][y], 0, n, 0, length);
                s[y] = n;
            }
            dest[x] = s;
        }
        return dest;
    }


    public int GetChildScoreAndUpdateKevin(int index, float score)
    {
        scores[index] = score;

        if (index < nbChildren - 1)
        {
            index++;
            moveHandler.UpdateAI(children[index]);
        }          
        else
        {
            index = 0;
            HandleEndOfGeneration();
            moveHandler.UpdateAI(children[index]);
        }

        return index;
    }


    private void HandleEndOfGeneration()
    {
        StoreScore();
        UpdateBests();
        StoreBestChildren();
        SelectGeneration();
    }

    public void UpdateBests()
    {
        bests = new Kevin[nbBest];

        List<float> scoresList = new List<float>(scores);
        for (int i = 0; i < nbBest; i++)
        {
            int indexMax = scoresList.IndexOf(scoresList.Max());
            Debug.Log("Best N°" + i + " is at index " + indexMax + " with a score of " + scoresList[indexMax]);
            bests[i] = children[indexMax];
            Debug.Log("Best N°" + i + " = " + bests[i]);
            scoresList[indexMax] = 0f;
        }

        scores = new float[nbChildren];
    }

    public void StoreScore()
    {
        Debug.Log("Entrée dans Store_Score() \n");

        String str = "";

        string path = Directory.GetCurrentDirectory() + "Score.txt";
        Debug.Log(path);

        for(int i = 0; i < nbChildren; i++)
        { 
            str = str + scores[i].ToString();
            str += "\n";
        }
        str += "*----*";

        if (!File.Exists(path))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                Debug.Log("No file found");
                sw.WriteLine("Début du fichier");
            }
        }

        // This text is always added, making the file longer over time
        // if it is not deleted.
        using (StreamWriter sw = new StreamWriter(path, true))
        {
            sw.WriteLine(str);
        }

        // Open the file to read from.
        using (StreamReader sr = File.OpenText(path))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                Console.WriteLine(s);
            }
        }

    }


    public void StoreBestChildren()
    {
        Debug.Log("Entrée dans Store_State() \n");

        String str = "";

        for(int i = 0; i < bests.Length; i++)
        {
            str += bests[i].ToString();
            str += "\n";
        }
        
        string path = Directory.GetCurrentDirectory() + "Dataset.txt";
        Debug.Log(path);

        // This text is added only once to the file.
        if (!File.Exists(path))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                Debug.Log("No file found");
                sw.WriteLine("Début du fichier");
            }
        }

        // This text is always added, making the file longer over time
        // if it is not deleted.
        using (StreamWriter sw = new StreamWriter(path, false))
        {
            sw.WriteLine(str);
        }

        // Open the file to read from.
        using (StreamReader sr = File.OpenText(path))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                Console.WriteLine(s);
            }
        }
    }


    private void GetBestsFromFile()
    {
        string path = Directory.GetCurrentDirectory() + "Dataset.txt";
        string text = System.IO.File.ReadAllText(path);

        // Example #2
        // Read each line of the file into a string array. Each element
        // of the array is one line of the file.
        string[] lines = System.IO.File.ReadAllLines(path);

        Debug.Log("First Weights from file : " + lines[0]);
        Debug.Log("First Biases from file : " + lines[1]);


        string[][] weightsString = new string[nbBest][];
        string[][] biasesString = new string[nbBest][];

        float[][] flatBestWeights = new float[nbBest][];
        float[][] flatBestBiases = new float[nbBest][];

        for (int i = 0; i < lines.Length - 1; i += 2)
        {
            // Weights
            string[] tmpWeights = lines[i].Split(' ');

            List<string> weightsList = new List<string>();
            for (int j = 0; j < tmpWeights.Length - 1; j++)
            {
                weightsList.Add(tmpWeights[j]);
            }
            weightsString[i / 2] = weightsList.ToArray();


            // Weights
            string[] tmpBiases = lines[i / 2 + 1].Split(' ');

            List<string> biasesList = new List<string>();
            for (int j = 0; j < tmpBiases.Length - 1; j++)
            {
                biasesList.Add(tmpBiases[j]);
            }
            biasesString[i / 2] = biasesList.ToArray();
        }


        for (int i = 0; i < weightsString.Length; i++)
        {
            List<float> flatWeights = new List<float>();
            for (int j = 0; j < weightsString[i].Length; j++)
            {
                flatWeights.Add(float.Parse(weightsString[i][j]));
            }
            flatBestWeights[i] = flatWeights.ToArray();
        }

        for (int i = 0; i < biasesString.Length; i++)
        {
            List<float> flatBiases = new List<float>();
            for (int j = 0; j < biasesString[i].Length; j++)
            {
                flatBiases.Add(float.Parse(biasesString[i][j]));
            }
            flatBestBiases[i] = flatBiases.ToArray();
        }

        Debug.Log("First Weights floated : " + flatBestWeights[0][0] + ", " + flatBestWeights[0][1] + ", " + flatBestWeights[0][2] + ", " + flatBestWeights[0][3]);
        Debug.Log("First Biases floated : " + flatBestBiases[0][0] + ", " + flatBestBiases[0][1] + ", " + flatBestBiases[0][2] + ", " + flatBestBiases[0][3]);

        for (int i = 0; i < nbBest; i++)
        {
            bests[i] = new Kevin(7, 8, 3, flatBestWeights[i], flatBestBiases[i]);
        }

        Debug.Log("Best Kevin : " + bests[0]);
    }
}
