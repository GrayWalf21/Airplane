using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
[System.Serializable]
public class Sample 
{
    public List<InputOFANN> sampleInputs = new List<InputOFANN>();
    public List<OutputOFANN> sampleOutputs = new List<OutputOFANN>();
}
[System.Serializable]
public class InputOFANN
{
    public float currentPlace_X;
    public float currentPlace_Y;
    public float currentPlace_Z;

    public float currentVelocity_X;
    public float currentVelocity_Y;
    public float currentVelocity_Z;

    public float runwayPlace_X;
    public float runwayPlace_Y;
    public float runwayPlace_Z;

    public float heightFrom_SeaLevel;
    public float heightFrom_CP;

    public float distanceFormRunway;
}

[System.Serializable]
public class OutputOFANN
{
    public float power;
    //Vertical
    public float rotation_X;
    //Perpendicular
    public float rotation_Y;
    //Horizontal
    public float rotation_Z;
}

