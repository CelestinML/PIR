using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UpdateGeneration
{
    private bool loadFromFile = false;

    private int nbChildren = 100;
    private int nbBest = 10;
    private int nbChildrenPerBest = 9;
    private int nbRandom = 10;

    public MoveWithAI moveHandler;
    public ShipSpawnerTraining shipSpawner;

    private Kevin[] children;
    private float[] scores;

    private Kevin[] bests;


    public UpdateGeneration()
    {
        children = new Kevin[nbChildren];
        scores = new float[nbChildren];

        bests = new Kevin[nbBest];
    }


    public Kevin[] CreateGeneration()
    {
        if(loadFromFile)
        {
            // TODO
            GetBestsFromFile();
            SelectGeneration();
            return children;
        }
        else
        {
            children = new Kevin[nbChildren];

            for (int i = 0; i < nbChildren; i++)
            {
                children[i] = new Kevin(7, 8, 3);
            }

            return children;
        }
    }


    private Kevin[] SelectGeneration()
    {
        int cpt = 0;

        for (int i = 0; i < nbBest; i++)
        {
            for(int j = 0; j < nbChildrenPerBest; j++)
            {
                // Kevin tmp = new Kevin();     
                Kevin tmp = new Kevin(Copy(bests[i].Weights), Copy(bests[i].Biases));
                tmp.Mutate();
                children[cpt] = tmp;
                cpt++;
            }
        }

        for (int i = 0; i < nbRandom; i++)
        {
            children[cpt] = new Kevin(7, 8, 3);
            cpt++;
        }

        return children;
    }


    public void GetChildScore(int index, float score)
    {
        scores[index] = score;
    }


    public Kevin[] HandleEndOfGeneration()
    {
        StoreScore();
        UpdateBests();
        StoreBestChildren();
        return SelectGeneration();
    }

    public void UpdateBests()
    {
        bests = new Kevin[nbBest];

        List<float> scoresList = new List<float>(scores);
        for (int i = 0; i < nbBest; i++)
        {
            int indexMax = scoresList.IndexOf(scoresList.Max());
            bests[i] = children[indexMax];
            scoresList[indexMax] = 0f;
        }

        scores = new float[nbChildren];
    }


    public void StoreScore()
    {
        String str = "";

        string path = Directory.GetCurrentDirectory() + "Score.txt";

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
        String str = "";

        for(int i = 0; i < bests.Length; i++)
        {
            str += bests[i].ToString();
            str += "\n";
        }
        
        string path = Directory.GetCurrentDirectory() + "Dataset.txt";

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

        string[] lines = System.IO.File.ReadAllLines(path);


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

        for (int i = 0; i < nbBest; i++)
        {
            bests[i] = new Kevin(7, 8, 3, flatBestWeights[i], flatBestBiases[i]);
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

    private static float[][] Copy(float[][] source)
    {
        float[][] dest = new float[source.Length][];
        for (int x = 0; x < source.Length; x++)
        {
            float[] s = new float[source[x].Length];
            int length = source[x].Length * sizeof(int);
            Buffer.BlockCopy(source[x], 0, s, 0, length);
            dest[x] = s;
        }
        return dest;
    }
}
