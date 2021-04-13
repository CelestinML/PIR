using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UpdateGeneration
{

    private int nbChildren = 10;
    private int nbBest = 3;
    private int nbChildrenPerBest = 3;
    private int nbRandom = 1;

    public MoveWithAI moveHandler;

    private Kevin[] children = new Kevin[10];
    private float[] scores = new float[10];

    private Kevin[] bests = new Kevin[3];



    public void CreateGeneration()
    {
        children = new Kevin[nbChildren];

        for(int i = 0; i < nbChildren; i++)
        {
            children[i] = new Kevin(7, 8, 3);
            Debug.Log("Kevin n" + i + " has been created");
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
            str += "\n\n\n";
        }

        str += "NEXT \n";
        
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
}
