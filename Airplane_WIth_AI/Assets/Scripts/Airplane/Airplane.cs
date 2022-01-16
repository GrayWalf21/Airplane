using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net.NetworkInformation;

public class Airplane : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private Transform forward;
    [SerializeField] private Transform up;
    [SerializeField] private Transform aFR;
    [SerializeField] private Transform aFL;
    [SerializeField] private Transform aBR;
    [SerializeField] private Transform aBL;
    [SerializeField] private Transform back_Aileron;

    [SerializeField] private float power = 0f;
    [SerializeField] private float accelaration = 0.05f;
    [SerializeField] private float maxForceOfEngine = 5000f;

    [SerializeField] private float startAngle = 0f;
    [SerializeField] private float startAngle_BackAileron = 0f;
    [SerializeField] private float angle = 15f;
    [SerializeField] private float maxAngle = 50f;
    [SerializeField] private float minAngle = -50f;

    [SerializeField] private float maxSpeed = 600f;

    [SerializeField] private float currentHeight = 0;
    [SerializeField] private float currentHeight_CP = 0;
    [SerializeField] private float maxHeight = 18000f;

    [SerializeField] private float densityOfAir = 10f;
    [SerializeField] private float currentVelocity = 0f;
    [SerializeField] private float cD = 0.05f;
    [SerializeField] private Vector3 areaOfAirplane = new Vector3(5f, 50f, 25f);

    private Arrow arrow;
    private Vector3 lastForce;
    private float distanceF;
    private WheelCollider wheelCollider;
    private RaycastHit hit;
    private bool canWrite = false;

    [HideInInspector] public Sample sample;
    [HideInInspector] public float[] currentInput;
    [HideInInspector] public float pw = 0;
    [HideInInspector] public float vt = 0;
    [HideInInspector] public float ht = 0;
    [HideInInspector] public float pt = 0;
    [HideInInspector] public bool driveManually = true;

    void Start()
    {
        //rb.centerOfMass = centerOfMass.position;
        angle = startAngle;
        arrow = gameObject.GetComponentInChildren<Arrow>();
        wheelCollider = gameObject.GetComponentInChildren<WheelCollider>();
        sample = new Sample();
    }

    void FixedUpdate()
    {
        currentVelocity = Mathf.Round(rb.velocity.magnitude);
        currentHeight = transform.position.y;

        if (Physics.Raycast(transform.position, -transform.forward, out hit,100000f))
        {
            distanceF = hit.distance;
        }
        else
        {
            distanceF = 100000f;
        }

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            currentHeight_CP = hit.distance - 2.80f;
        }

        var disT = Vector3.Distance(transform.position, arrow.airport.transform.position);

        //Open this
        GetCurrentInput(currentHeight, currentHeight_CP, disT);

        double[] answer = new double[10];

        if (!driveManually)
        {
            /*currentInput = new float[24];
            currentInput = new float[]{ 0 ,  0 ,  1 ,  0 ,  1 ,  1,   0,   0 ,  1  , 0 ,  0 ,  1  , 0 ,  0 ,  1 };
            float[] correctAnswer = new float[] { 0,0,0,0,0,0,0,0,0,1 };*/
            answer = currentInput.GetOutput(FileManager.Instance.output);

            /*var max = Mathf.NegativeInfinity;
            var id = 0;
            for(int i=0; i<10; i++)
            {
                if (max < (float)answer[i]) 
                {
                    max = (float)answer[i];
                    id = i;
                }
            }

            print("id: " + id + " Max: " + max);*/
            var max = Mathf.NegativeInfinity;
            var max2 = Mathf.NegativeInfinity;
            var id = 0;
            var id2 = 0;
            for(int i=0; i<4; i++)
            {
                if (max < (float)answer[i]) 
                {
                    max2 = max;
                    id2 = id;
                    max = (float)answer[i];
                    id = i;
                }
            }
            /*if(id == 0) pw = (float) answer[0];
            else if(id == 1) vt = (float)answer[1];
            else if(id == 2) pt = (float)answer[2];
            else if(id == 3) ht = (float)answer[3];

            if(id2 == 0) pw = (float) answer[0];
            else if(id2 == 1) vt = (float)answer[1];
            else if(id2 == 2) pt = (float)answer[2];
            else if(id2 == 3) ht = (float)answer[3];*/

            float pwO = 10f;

            pw = (float)answer[0];
            if ((float)answer[1] > 0.5f) vt = (float)answer[1];
            if ((float)answer[2] > 0.5f) pt = (float)answer[2];
            if ((float)answer[3] > 0.5f) ht = (float)answer[3];

            /*pw = (float)answer[0] * pwO;
            vt = (float)answer[1];
            pt = (float)answer[2];
            ht = (float)answer[3];*/

            print(pw + " " + vt + " " + pt + " " + ht);
        }

        // CalculatePowerAndDensity(Input.GetAxis("Power"));
        CalculatePowerAndDensity(pw);

        //Debug.DrawRay(transform.position,-transform.forward * 100000f, Color.green);
        var aR = CalculateAirDrag(rb.velocity);

        //print(rb.velocity);
        //transform.Rotate(new Vector3(Input.GetAxis("Vertical"),0,0));

        /*/
            var vt = Input.GetAxis("Vertical");
        var ht = Input.GetAxis("Horizontal");
        var pt = Input.GetAxis("Perpendicular");*/

        var angle_X = RotateOnX(vt);
        var angle_Y = RotateOnY(pt);
        var angle_Z = RotateOnZ(ht);

        RotateAilerons(vt, ht, pt);

        //print(transform.forward);
        AddForce(ht,aR);

        /*/if (Input.GetKeyDown(KeyCode.Keypad0)) StopEngine();
        if (Input.GetKeyDown(KeyCode.F6))
        {
            PlayerPrefs.SetInt("Index", 0);
            FileManager.Instance.index = 0;
            print("index = 0");
        }
        if (Input.GetKeyDown(KeyCode.T)) ClearEverything();
        if (Input.GetKeyDown(KeyCode.F)) StartForce();
        if (Input.GetKeyDown(KeyCode.F9)) FileManager.Instance.SavePlayerData();
        if (Input.GetKeyDown(KeyCode.F7) && !canWrite)
        {
            canWrite = true;
            print("canWrite = true");
        }
        else if (Input.GetKeyDown(KeyCode.F7) && canWrite)
        {
            canWrite = false;
            print("canWrite = false");
        }*/


        uiManager.Instance.SetText(0,(Mathf.Round(currentVelocity)).ToString());
        uiManager.Instance.SetText(1, Mathf.Round(currentHeight).ToString());
        uiManager.Instance.SetText(2,power.ToString());
        uiManager.Instance.SetText(3, disT.ToString());
        uiManager.Instance.SetText(4, currentHeight_CP.ToString("0."));

        if (canWrite)
        {
            WriteInputsAndOutputs(currentHeight, currentHeight_CP, disT, vt, pt, ht);
            FileManager.Instance.sample = sample;
        }
    }
    private void GetCurrentInput(float heightFrom_SeaLevel, float heightFrom_CP, float distanceFormRunway)
    {
        float cpX = 36360.85f;                      
        float cpY = 8225.081f;
        float cpZ = 20170.21f;

        float cvX = 282.7596f;
        float cvY = 276.7364f;
        float cvZ = 273.1143f;

        float rX = 0.9350824f;
        float rY = 0.9993166f;
        float rZ = 0.8446074f;

        float apX = 1476f;

        float hfS = 8225.081f;
        float hfL = 7768.037f;
        float dR = 37661.55f;
        float dF = 100000f;

        currentInput = new float[16];

        currentInput[0] = transform.position.x / cpX;
        currentInput[1] = transform.position.y / cpY;
        currentInput[2] = transform.position.z / cpZ;

        currentInput[3] = rb.velocity.x / cvX;
        currentInput[4] = rb.velocity.y / cvY;
        currentInput[5] = rb.velocity.z / cvZ;

        currentInput[6] = transform.rotation.x / rX;
        currentInput[7] = transform.rotation.y / rY;
        currentInput[8] = transform.rotation.z / rZ;

        currentInput[9] =  arrow.airport.transform.position.x / apX;
        currentInput[10] = arrow.airport.transform.position.y;
        currentInput[11] = arrow.airport.transform.position.z;

        currentInput[12] = heightFrom_SeaLevel / hfS;
        currentInput[13] = heightFrom_CP / hfL;
        currentInput[14] = distanceFormRunway / dR;
        currentInput[15] = distanceF / dF;
    }

    private void WriteInputsAndOutputs(float heightFrom_SeaLevel, float heightFrom_CP, float distanceFormRunway,float rotation_X,float rotation_Y, float rotation_Z)
    {
        InputOFANN input = new InputOFANN();
        OutputOFANN output = new OutputOFANN();

        input.currentPlace_X = transform.position.x;
        input.currentPlace_Y = transform.position.y;
        input.currentPlace_Z = transform.position.z;

        input.currentVelocity_X = rb.velocity.x;
        input.currentVelocity_Y = rb.velocity.y;
        input.currentVelocity_Z = rb.velocity.z;

        input.currentRotation_X = transform.rotation.x;
        input.currentRotation_Y = transform.rotation.y;
        input.currentRotation_Z = transform.rotation.z;

        input.runwayPlace_X = arrow.airport.transform.position.x;
        input.runwayPlace_Y = arrow.airport.transform.position.y;
        input.runwayPlace_Z = arrow.airport.transform.position.z;

        input.heightFrom_SeaLevel = heightFrom_SeaLevel;
        input.heightFrom_CP = heightFrom_CP;
        input.distanceFormRunway = distanceFormRunway;
        input.distanceF = distanceF;

        output.power = power;
        output.rotation_X = rotation_X;
        output.rotation_Y = rotation_Y;
        output.rotation_Z = rotation_Z;


        sample.sampleInputs.Add(input);
        sample.sampleOutputs.Add(output);
    }

    public void StartForce()
    {
        wheelCollider.motorTorque = 5f;
        StartCoroutine(StopTorque(0.2f));
    }

    IEnumerator StopTorque(float time)
    {
        yield return new WaitForSeconds(time);
        wheelCollider.motorTorque = 0;
    }
    public void ClearEverything()
    {
        rb.velocity = Vector3.zero;
        angle = 0f;
    }

    public void StopEngine()
    {
        power = 0;
        var force = lastForce ;
        var forceMode = ForceMode.Force;
        StartCoroutine(GiveForceAfter( force, forceMode, 0.2f));
    }

    IEnumerator GiveForceAfter(Vector3 force, ForceMode forceMode, float time)
    {
        yield return new WaitForSeconds(time);
        rb.AddForce(force, forceMode);
        StopAllCoroutines();
    }

    private Vector3 CalculateAirDrag(Vector3 velocity)
    {
        var direction = velocity.normalized * (-1);

        var cAAX = areaOfAirplane.x * Mathf.Abs(transform.forward.x) + areaOfAirplane.y * Mathf.Abs(transform.up.x) + areaOfAirplane.z * Mathf.Abs(transform.right.x);
        var cAAY = areaOfAirplane.x * Mathf.Abs(transform.forward.y) + areaOfAirplane.y * Mathf.Abs(transform.up.y) + areaOfAirplane.z * Mathf.Abs(transform.right.y);
        var cAAZ = areaOfAirplane.x * Mathf.Abs(transform.forward.z) + areaOfAirplane.y * Mathf.Abs(transform.up.y) + areaOfAirplane.z * Mathf.Abs(transform.right.z);

        cAAX = Mathf.Abs(cAAX);
        cAAY = Mathf.Abs(cAAY);
        cAAZ = Mathf.Abs(cAAZ);

        var aRX = cD * ((densityOfAir * velocity.x * velocity.x) / 2) * cAAX;
        var aRY = cD * ((densityOfAir * velocity.y * velocity.y) / 2) * cAAY;
        var aRZ = cD * ((densityOfAir * velocity.z * velocity.z) / 2) * cAAZ;

        var aR = new Vector3(aRX*direction.x, aRY* direction.y, aRZ* direction.z);
        return aR;
    }
    private void AddForce(float ht,Vector3 aR )
    {

        ForceMode forceMode = ForceMode.Impulse;

        angle = Mathf.Clamp(angle + ht, -50f, 50f);

        var directionH = -transform.position.DirectionTo(forward.position);
        var directionV = -transform.position.DirectionTo(up.position);

        /*var stH_X = directionH.x.ToString("0.00");
        var stH_Y = directionH.y.ToString("0.00");
        var stH_Z = directionH.z.ToString("0.00");

        var stV_X = directionV.x.ToString("0.00");
        var stV_Y = directionV.y.ToString("0.00");
        var stV_Z = directionV.z.ToString("0.00");

        var H_x = float.Parse(stH_X);
        var H_y = float.Parse(stH_Y);
        var H_z = float.Parse(stH_Z);

        var V_x = float.Parse(stV_X);
        var V_y = float.Parse(stV_Y);
        var V_z = float.Parse(stV_Z);

        directionH = new Vector3(H_x, H_y, H_z);
        directionV = new Vector3(V_x, V_y, V_z);*/

        //print(directionH + " " + directionV);

        var vX = rb.velocity.x * directionH.x + rb.velocity.y * directionH.y + rb.velocity.z * directionH.z;
        //print("X: " + direction.x + ",Y: " + direction.y + ",Z: " + direction.z);
        //print("X: " + direction.x.ToString("0.00") + ",Y: " + direction.y.ToString("0.00") + ",Z: " + direction.z.ToString("0.00"));
        //print("Direction: " + direction + ", Power: " + power + ", MaxForce: " + maxForceOfEngine + ", DensityofAir: " + densityOfAir);

        var fH = directionH * power * maxForceOfEngine * densityOfAir;
        var fV = directionV * power * maxForceOfEngine * densityOfAir * Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad)) * vX /8000;

        //var fT = fH + fV;
        //Debug.Log(fT);
        //print("Horizontal: "+fH +" Forward: "+ -transform.forward);
        //print("Horizontal: "+fH +" Vertical: "+fV+" Air drag: " + aR);
        if(power > 1.5f)
        {
            //lastForce = fH + fV;
            lastForce = fH;
        }
        else
        {
            forceMode = ForceMode.Force;
        }

        fV = Vector3.zero;

        if (fH + aR+ fV != Vector3.zero || fH + aR + fV != null)
            //rb.AddForce(fH+aR);
            //rb.AddForce(fH,ForceMode.Impulse);
            rb.AddForce(fH + aR + fV,forceMode);
    }
    private void CalculatePowerAndDensity(float powerInput)
    {

        densityOfAir = Mathf.Clamp(10f * ((maxHeight - currentHeight) / maxHeight), 0, 10);

        power = Mathf.Clamp(power + (accelaration * powerInput), -5f, 10f);
    }
    private float RotateOnX(float value_V)
    {
        var v = Mathf.Abs(
               transform.forward.x * rb.velocity.x +
               transform.forward.y * rb.velocity.y +
               transform.forward.z * rb.velocity.z);

        var angle = value_V * v / 500;
        angle = Mathf.Clamp(angle,-1,1);

        transform.Rotate(new Vector3(-angle, 0, 0), Space.Self);
        return angle;
    }
    private float RotateOnY(float value_P)
    {
        var v = Mathf.Abs(
               transform.forward.x * rb.velocity.x +
               transform.forward.y * rb.velocity.y +
               transform.forward.z * rb.velocity.z);

        var angle = value_P * v / 500;
        angle = Mathf.Clamp(angle, -1, 1);

        transform.Rotate(new Vector3(0, angle, 0), Space.Self);
        return angle;
    }
    private float RotateOnZ(float value_H)
    {
        var v = Mathf.Abs(
               transform.forward.x * rb.velocity.x +
               transform.forward.y * rb.velocity.y +
               transform.forward.z * rb.velocity.z);

        var angle = value_H * v / 500;
        angle = Mathf.Clamp(angle, -1, 1);

        transform.Rotate(new Vector3(0, 0, angle), Space.Self);
        return angle;
    }
    private void RotateAilerons(float value_V, float value_H, float value_P)
    {
        //print(aFR.localRotation.eulerAngles);
        aFR.localRotation = Quaternion.Euler(new Vector3(Mathf.LerpUnclamped(startAngle, maxAngle, value_H), 0, 52.5f));
        aFL.localRotation = Quaternion.Euler(new Vector3(Mathf.LerpUnclamped(startAngle, maxAngle, value_H), 0, -52.5f));

        /*aFR.Rotate(new Vector3(0, 0, -value_H), Space.Self);
        aFL.Rotate(new Vector3(0, 0, value_H), Space.Self);*/

        /*aBR.Rotate(new Vector3(0, 0, value_V), Space.Self);
        aBL.Rotate(new Vector3(0, 0, value_V), Space.Self);*/

        aBR.localRotation = Quaternion.Euler(new Vector3(0, Mathf.LerpUnclamped(startAngle, maxAngle, -value_V), 0));
        aBL.localRotation = Quaternion.Euler(new Vector3(0, Mathf.LerpUnclamped(startAngle, maxAngle, -value_V), 0));
        
        back_Aileron.localRotation = Quaternion.Euler(new Vector3(0, 28.2f, -Mathf.LerpUnclamped(startAngle_BackAileron, maxAngle, value_P)));

        //LimitRotations();
    }
    /*private void LimitRotations()
    {
        var aFRE = aFR.localRotation.eulerAngles;
        var aFLE = aFL.localRotation.eulerAngles;
         
        var aBRE = aBR.localRotation.eulerAngles;
        var aBLE = aBL.localRotation.eulerAngles;

        aFRE.z = (aFRE.z > 180) ? aFRE.z - 360 : aFRE.z;
        aFRE.z = Mathf.Clamp(aFRE.z,minAngle,maxAngle);
        aFR.localRotation = Quaternion.Euler(aFRE);

        aFLE.z = (aFLE.z > 180) ? aFLE.z - 360 : aFLE.z;
        aFLE.z = Mathf.Clamp(aFLE.z, minAngle, maxAngle);
        aFL.localRotation = Quaternion.Euler(aFLE);

        aBRE.z = (aBRE.z > 180) ? aBRE.z - 360 : aBRE.z;
        aBRE.z = Mathf.Clamp(aBRE.z, minAngle, maxAngle);
        aBR.localRotation = Quaternion.Euler(aBRE);

        aBLE.z = (aBLE.z > 180) ? aBLE.z - 360 : aBLE.z;
        aBLE.z = Mathf.Clamp(aBLE.z, minAngle, maxAngle);
        aBL.localRotation = Quaternion.Euler(aBLE);
    }*/
}
