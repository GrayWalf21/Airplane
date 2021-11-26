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

    [SerializeField] private float power = 0f;
    [SerializeField] private float accelaration = 2f;
    [SerializeField] private float maxForce_V = 100f;

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
        densityOfAir = Mathf.Clamp(100f * ((maxHeight - currentHeight) / maxHeight), 0, 100);

        

        power = Mathf.Clamp(power + (accelaration * Input.GetAxis("Vertical")), -50f, 100f);

        var angleIncreation = Input.GetAxis("Horizontal");
        angle = Mathf.Clamp(angle + angleIncreation, -50f, 50f);

        var direction = centerOfMass.position.DirectionTo(front.position);
        var fH = power* maxForce_V *100* Vector3.left ;
        var fV = Vector3.up * power * maxForce_V * Mathf.Tan(angle * Mathf.Deg2Rad) * densityOfAir;

        var fT = fH + fV;
        //Debug.Log(fT);
        rb.AddForce(fT);

        uiManager.Instance.SetText(0,rb.velocity.magnitude.ToString());
        uiManager.Instance.SetText(1,currentHeight.ToString());
        uiManager.Instance.SetText(2,power.ToString());
    }
}
