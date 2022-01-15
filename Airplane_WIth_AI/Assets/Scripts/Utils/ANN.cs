using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ANN
{
    public static Double[] GetOutput(this float[] input, string[] nw)
    {
        //float[] answer = new float[4];

        
        if (nw == null) return null;

        var nf = nw[1].Split(' ');
        var nI = Convert.ToInt32(nf[0]);
        var nH = Convert.ToInt32(nf[1]);
        var nO = Convert.ToInt32(nf[2]);
        Debug.Log(nI + " + " + nO + " + " +nH);

        double[][] WCommand1 = new double[nH][];
        double[][] WCommand2 = new double[nO][];

        int index = 2;
        int count = 0;

        double[] act1 = new double[nI];
        double[] act2 = new double[nI];

        for (int i = 0; i < nH; i++) 
            for (int j = 0; j < nI+1; j++)
            {
                WCommand1[i][j] = Convert.ToDouble(nw[index + count] );
                count++;
            }

        for (int i = 0; i < nO; i++) 
            for (int j = 0; j < nH + 1; j++)
            {
                WCommand2[i][j] = Convert.ToDouble(nw[index + count]);
                count++;
            }

        for (int i = 0; i < nH; i++)
        {
            act1[i] = WCommand1[i][nI];

            for (int j = 0; j < nI; j++) 
                act1[i] += WCommand1[i][j] * act2[j];

            act1[i] = sigmoid(act1[i]);
        }
        for (int i = 0; i < nO; i++)
        {
            act2[i] = WCommand2[i][nH];

            for (int j = 0; j < nH; j++) 
                act2[i] += WCommand2[i][j] * act1[j];

            act2[i] = sigmoid(act2[i]);
        }
        //answer = act2;
        return act2;
    }

    private static double sigmoid(double x)
    {
        return 1.0 / (1.0 + Mathf.Exp((float) -x));
    }
}
