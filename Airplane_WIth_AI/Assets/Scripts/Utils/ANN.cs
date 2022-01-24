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
        var nI = Convert.ToInt32(nf[0]);//18
        var nH = Convert.ToInt32(nf[1]);//16
        var nO = Convert.ToInt32(nf[2]);//4
        Debug.Log(nI + " + " + nO + " + " +nH);

        double[,] WCommand1 = new double[nH,nI+1];//16,19
        double[,] WCommand2 = new double[nO,nH+1];//4,18

        int index = 2;
        int count = 0;

        double[] act1 = new double[nH + nI];//34
        double[] act2 = new double[nH + nI];//34

        for(int i = 0; i< nI; i++)//18
        {
            act2[i] = (double) input[i];
        }

        for (int i = 0; i < nH; i++) //16
            for (int j = 0; j < nI+1; j++)//19  -> 16,19
            {
                WCommand1[i,j] = Convert.ToDouble(nw[index + count]);
                count++;
            }

        for (int i = 0; i < nO; i++)//4
            for (int j = 0; j < nH + 1; j++)//18 -> 4,18
            {
                WCommand2[i,j] = Convert.ToDouble(nw[index + count]);
                count++;
            }

        for (int i = 0; i < nH; i++)//16
        {
            act1[i] = WCommand1[i,nI];
            //bias + x*w + x*w
            for (int j = 0; j < nI; j++) //18
                act1[i] += WCommand1[i,j] * act2[j];

            act1[i] = sigmoid(act1[i]);
        }
        for (int i = 0; i < nO; i++)//4
        {
            act2[i] = WCommand2[i,nH];

            for (int j = 0; j < nH; j++)//16 
                act2[i] += WCommand2[i,j] * act1[j];

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
