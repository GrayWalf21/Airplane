using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ANN
{
    public static float[] GetOutput(this float[] input)
    {
        float[] answer = new float[4];

        string[] nw = FileManager.Instance.output;

        var nf = nw[1].Split(' ');
        var ni = Convert.ToInt32(nf[0]);
        Debug.Log(ni);
        return answer;
    }
}
