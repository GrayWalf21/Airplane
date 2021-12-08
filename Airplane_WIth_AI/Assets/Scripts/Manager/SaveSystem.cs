using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public static class SaveSystem
{
    public static void SaveData(Sample sample)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.streamingAssetsPath + "/Sample.txt";
        //FileStream stream = new FileStream(path, FileMode.Create);

        var file = File.CreateText(path);
        //StreamWriter stream = new StreamWriter(path);
        //PlayerData data = new PlayerData(player);
        var count = sample.sampleInputs.Count;
        for (int i = 0; i < count; i++)
        {
            var line = "";

            if (FileManager.Instance.first)
            {
                FileManager.Instance.first = false;
                line = count.ToString() + " " + "12" +" "+ "4";
                file.WriteLine(line + Environment.NewLine);
            }

            //First 12th is input and last 4th is output
            line =

            sample.sampleInputs[i].currentPlace_X.ToString()        + " " +
            sample.sampleInputs[i].currentPlace_Y.ToString()        + " " +
            sample.sampleInputs[i].currentPlace_Z.ToString()        + " " +

            sample.sampleInputs[i].currentVelocity_X.ToString()     + " " +
            sample.sampleInputs[i].currentVelocity_Y.ToString()     + " " +
            sample.sampleInputs[i].currentVelocity_Z.ToString()     + " " +

            sample.sampleInputs[i].runwayPlace_X.ToString()         + " " +
            sample.sampleInputs[i].runwayPlace_Y.ToString()         + " " +
            sample.sampleInputs[i].runwayPlace_Z.ToString()         + " " +

            sample.sampleInputs[i].heightFrom_SeaLevel.ToString()   + " " +
            sample.sampleInputs[i].heightFrom_CP.ToString()         + " " +
            sample.sampleInputs[i].distanceFormRunway.ToString()    + " " + " "+

            sample.sampleOutputs[i].power.ToString()                + " " +
            sample.sampleOutputs[i].rotation_X.ToString()           + " " +
            sample.sampleOutputs[i].rotation_Y.ToString()           + " " +
            sample.sampleOutputs[i].rotation_Z.ToString();

            //File.WriteAllText(path, line+Environment.NewLine);

            file.WriteLine(line+Environment.NewLine);
        }
        file.Close();
        //formatter.Serialize(stream, sample);
        //stream.Close();
    }
    public static Sample Loadata()
    {
        string path = Application.streamingAssetsPath + "/Sample.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Sample data = formatter.Deserialize(stream) as Sample;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("No player Data !" + path);
            return null;
        }
    }
}
