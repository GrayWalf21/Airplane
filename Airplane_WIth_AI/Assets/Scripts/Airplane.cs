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
    private WheelCollider wheelCollider;
    private RaycastHit hit;

    void Start()
    {
        //rb.centerOfMass = centerOfMass.position;
        angle = startAngle;
        arrow = gameObject.GetComponentInChildren<Arrow>();
        wheelCollider = gameObject.GetComponentInChildren<WheelCollider>();
    }

    void FixedUpdate()
    {
        currentVelocity = Mathf.Round(rb.velocity.magnitude);
        currentHeight = transform.position.y;
        CalculatePowerAndDensity(Input.GetAxis("Power"));

        if (Physics.Raycast(transform.position,Vector3.down,out hit))
        {
            currentHeight_CP = hit.distance;
        }

        var aR = CalculateAirDrag(rb.velocity);

        //print(rb.velocity);
        //transform.Rotate(new Vector3(Input.GetAxis("Vertical"),0,0));
        var vt = Input.GetAxis("Vertical");
        var ht = Input.GetAxis("Horizontal");
        var pt = Input.GetAxis("Perpendicular");

        RotateOnX(vt);
        RotateOnY(pt);
        RotateOnZ(ht);
        RotateAilerons(vt, ht, pt);

        //print(transform.forward);
        AddForce(ht,aR);

        if (Input.GetKey(KeyCode.Keypad0)) StopEngine();
        if (Input.GetKey(KeyCode.T)) ClearEverything();
        if (Input.GetKey(KeyCode.F)) StartForce();

        var disT = Vector3.Distance(transform.position, arrow.airport.transform.position);

        uiManager.Instance.SetText(0,(Mathf.Round(currentVelocity)).ToString());
        uiManager.Instance.SetText(1, Mathf.Round(currentHeight).ToString());
        uiManager.Instance.SetText(2,power.ToString());
        uiManager.Instance.SetText(3, disT.ToString());
        uiManager.Instance.SetText(4, currentHeight_CP.ToString());
    }

    private void StartForce()
    {
        wheelCollider.motorTorque = 5f;
        StartCoroutine(StopTorque(0.2f));
    }

    IEnumerator StopTorque(float time)
    {
        yield return new WaitForSeconds(time);
        wheelCollider.motorTorque = 0;
    }
    private void ClearEverything()
    {
        rb.velocity = Vector3.zero;
        angle = 0f;
    }

    private void StopEngine()
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

        var cAAX = areaOfAirplane.x * transform.forward.x + areaOfAirplane.y * transform.up.x + areaOfAirplane.z * transform.right.x;
        var cAAY = areaOfAirplane.x * transform.forward.y + areaOfAirplane.y * transform.up.y + areaOfAirplane.z * transform.right.y;
        var cAAZ = areaOfAirplane.x * transform.forward.z + areaOfAirplane.y * transform.up.y + areaOfAirplane.z * transform.right.z;

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
    private void RotateOnX(float value_V)
    {
        var v = Mathf.Abs(
               transform.forward.x * rb.velocity.x +
               transform.forward.y * rb.velocity.y +
               transform.forward.z * rb.velocity.z);

        var angle = value_V * v / 500;
        angle = Mathf.Clamp(angle,-1,1);

        transform.Rotate(new Vector3(-angle, 0, 0), Space.Self);
    }
    private void RotateOnY(float value_P)
    {
        var v = Mathf.Abs(
               transform.forward.x * rb.velocity.x +
               transform.forward.y * rb.velocity.y +
               transform.forward.z * rb.velocity.z);

        var angle = value_P * v / 500;
        angle = Mathf.Clamp(angle, -1, 1);

        transform.Rotate(new Vector3(0, angle, 0), Space.Self);
    }
    private void RotateOnZ(float value_H)
    {
        var v = Mathf.Abs(
               transform.forward.x * rb.velocity.x +
               transform.forward.y * rb.velocity.y +
               transform.forward.z * rb.velocity.z);

        var angle = value_H * v / 500;
        angle = Mathf.Clamp(angle, -1, 1);

        transform.Rotate(new Vector3(0, 0, angle), Space.Self);
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
