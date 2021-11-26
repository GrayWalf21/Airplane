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
    [SerializeField] private float maxForce = 1000f;
    [SerializeField] private float angle = 15f;
    [SerializeField] private float maxAngle = 50f;
    [SerializeField] private float minAngle = -50f;
    [SerializeField] private float maxSpeed = 320f;

    void Start()
    {
        //rb.centerOfMass = centerOfMass.position;
    }

    void Update()
    {
        if (rb.velocity.magnitude >= maxSpeed) return;

        power = Mathf.Clamp(power+Input.GetAxis("Vertical"),-50f,100f);

        var angleIncreation = Input.GetAxis("Horizontal");
        angle = Mathf.Clamp(angle + angleIncreation, -50f, 50f);

        var direction = -centerOfMass.localPosition.DirectionTo(front.localPosition);
        var fH = power * direction * maxForce;
        var fT =  -fH * Mathf.Tan(angle * Mathf.Deg2Rad);

        Debug.Log(fT);
        rb.AddForce(fT);
        uiManager.Instance.SetText(rb.velocity.magnitude.ToString() );
        
    }
}
