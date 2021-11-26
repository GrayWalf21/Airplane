using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Airplane : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private Transform front;
    [SerializeField] private Transform aFR;
    [SerializeField] private Transform aFL;
    [SerializeField] private Transform aBR;
    [SerializeField] private Transform aBL;

    [SerializeField] private float power = 0f;
    [SerializeField] private float accelaration = 0.5f;
    [SerializeField] private float maxForce_V = 50f;

    [SerializeField] private float angle = 15f;
    [SerializeField] private float maxAngle = 50f;
    [SerializeField] private float minAngle = -50f;

    [SerializeField] private float maxSpeed = 320f;

    [SerializeField] private float currentHeight = 0;
    [SerializeField] private float maxHeight = 18000;

    [SerializeField] private float densityOfAir =100f;

    void Start()
    {
        //rb.centerOfMass = centerOfMass.position;
    }

    void Update()
    {
        //transform.Rotate(new Vector3(Input.GetAxis("Vertical"),0,0));
        var vt = Input.GetAxis("Vertical");
        var ht = Input.GetAxis("Horizontal");

        RotateOnX(vt);
        RotateOnZ(ht);
        RotateAilerons(vt, ht);

        CalculatePowerAndDensity(Input.GetAxis("Power"));

        AddForce(ht);

        uiManager.Instance.SetText(0,(Mathf.Round(rb.velocity.magnitude)).ToString());
        uiManager.Instance.SetText(1,currentHeight.ToString());
        uiManager.Instance.SetText(2,power.ToString());
    }

    private void AddForce(float ht)
    {
        angle = Mathf.Clamp(angle + ht, -50f, 50f);

        var direction = -aBR.position.DirectionTo(aBR.GetChild(1).transform.position);
        var fH = power * maxForce_V * 100 * direction;
        var fV = Vector3.up * power * maxForce_V * Mathf.Tan(angle * Mathf.Deg2Rad) * densityOfAir;

        var fT = fH + fV;
        //Debug.Log(fT);

        rb.AddForce(fH);
    }

    private void CalculatePowerAndDensity(float powerInput)
    {

        densityOfAir = Mathf.Clamp(100f * ((maxHeight - currentHeight) / maxHeight), 0, 100);

        power = Mathf.Clamp(power + (accelaration * powerInput), -50f, 100f);
    }
    private void RotateOnX(float value_V)
    {
        var angle = value_V * rb.velocity.magnitude;
        angle = Mathf.Clamp(angle,-1,1);

        transform.Rotate(new Vector3(-angle, 0, 0));
    }
    private void RotateOnZ(float value_H)
    {
        var angle = value_H * rb.velocity.magnitude;
        angle = Mathf.Clamp(angle, -1, 1);

        transform.Rotate(new Vector3(0, 0, angle));
    }
    private void RotateAilerons(float value_V,float value_H)
    {
        //print(aFR.localRotation.eulerAngles);
        aFR.Rotate(new Vector3(0, 0, -value_H), Space.Self);
        aFL.Rotate(new Vector3(0, 0, value_H), Space.Self);

        aBR.Rotate(new Vector3(0, 0, value_V), Space.Self);
        aBL.Rotate(new Vector3(0, 0, value_V), Space.Self);

        LimitRotations();
    }
    private void LimitRotations()
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
    }
}
