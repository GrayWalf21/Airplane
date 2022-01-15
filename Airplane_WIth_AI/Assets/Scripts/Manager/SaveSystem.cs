using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.Runtime.InteropServices;

public static class SaveSystem
{
    public static void SaveData(Sample sample)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        var index = FileManager.Instance.index;  

        string path = Application.streamingAssetsPath + "/Sample"+index.ToString()+".txt";
        //FileStream stream = new FileStream(path, FileMode.Create);

        var file = File.CreateText(path);
        //StreamWriter stream = new StreamWriter(path);
        //PlayerData data = new PlayerData(player);
        var count = sample.sampleInputs.Count;

        for (int i = -1; i < count; i++)
        {
            var line = "";

            Debug.Log(FileManager.Instance.first);
            if (FileManager.Instance.first)
            {
                line = count.ToString() + " " + "16" +" "+ "4";
                file.WriteLine(line);
                FileManager.Instance.first = false;
                continue;
            }
            else
            {
                //First 12th is input and last 4th is output
                line =

                sample.sampleInputs[i].currentPlace_X.ToString() + "\t" +
                sample.sampleInputs[i].currentPlace_Y.ToString() + "\t" +
                sample.sampleInputs[i].currentPlace_Z.ToString() + "\t" +

                sample.sampleInputs[i].currentVelocity_X.ToString() + "\t" +
                sample.sampleInputs[i].currentVelocity_Y.ToString() + "\t" +
                sample.sampleInputs[i].currentVelocity_Z.ToString() + "\t" +

                sample.sampleInputs[i].currentRotation_X.ToString() + "\t" +
                sample.sampleInputs[i].currentRotation_Y.ToString() + "\t" +
                sample.sampleInputs[i].currentRotation_Z.ToString() + "\t" +

                sample.sampleInputs[i].runwayPlace_X.ToString() + "\t" +
                sample.sampleInputs[i].runwayPlace_Y.ToString() + "\t" +
                sample.sampleInputs[i].runwayPlace_Z.ToString() + "\t" +

                sample.sampleInputs[i].heightFrom_SeaLevel.ToString() + "\t" +
                sample.sampleInputs[i].heightFrom_CP.ToString() + "\t" +
                sample.sampleInputs[i].distanceFormRunway.ToString() + "\t" + 
                sample.sampleInputs[i].distanceF.ToString() + "\t" + 
                //"\t" +

                sample.sampleOutputs[i].power.ToString() + "\t" +
                sample.sampleOutputs[i].rotation_X.ToString() + "\t" +
                sample.sampleOutputs[i].rotation_Y.ToString() + "\t" +
                sample.sampleOutputs[i].rotation_Z.ToString();

                //File.WriteAllText(path, line+Environment.NewLine);

                file.WriteLine(line);
            }
        }
        file.Close();
        //formatter.Serialize(stream, sample);
        //stream.Close();
    }
    public static string[] Loadata()
    {
        string path = Application.streamingAssetsPath + "/Samplea.nw";
        if (File.Exists(path))
        {
            //BinaryFormatter formatter = new BinaryFormatter();
            //FileStream stream = new FileStream(path, FileMode.Open);

            //Sample data = formatter.Deserialize(stream) as Sample;
           
            var file = File.ReadAllLines(path);
            var nf = file[1].Split(' ');
            var ni = Convert.ToInt32(nf[0]);

            Debug.Log("Output 1 : " + ni);

            //stream.Close();

            return file;
        }
        else
        {
            Debug.Log("No player Data !" + path);
            return null;
        }
    }
}
