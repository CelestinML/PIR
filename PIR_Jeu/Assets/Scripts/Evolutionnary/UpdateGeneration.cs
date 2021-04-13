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

        for (int i = 0; i < nbBest; i++)
        {
            for(int j = 0; j < nbChildrenPerBest; j++)
            {
                Kevin tmp = new Kevin(bests[i].Shape[0], bests[i].Shape[1], bests[i].Shape[bests[i].Shape.Length - 1], bests[i].Weights, bests[i].Biases);
                tmp_Children[(i * nbBest) + j] = tmp;
            }
        }

        for (int i = 0; i < nbRandom; i++)
        {
            tmp_Children[nbBest * nbChildrenPerBest + i] = new Kevin(7, 8, 3);
        }

        for (int i = 0; i < tmp_Children.Length; i++)
        {
            tmp_Children[i].Mutate();
            Debug.Log(tmp_Children[i]);
            children[i] = tmp_Children[i];
        }

        for (int i = 0; i < tmp_Children.Length; i++)
        {
            Debug.Log("Children[" + i + "] = " + children[i]);
        }
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

        string path = @"C:\\Users\\xSyl0\\OneDrive\\Bureau\\PIR\\PIR\\PIR_Jeu\\Assets\\Dataset\\Score.txt";

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
        
        string path = @"C:\\Users\\xSyl0\\OneDrive\\Bureau\\PIR\\PIR\\PIR_Jeu\\Assets\\Dataset\\Dataset.txt";
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
