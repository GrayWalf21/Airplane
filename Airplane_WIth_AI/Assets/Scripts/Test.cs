using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float force = 20f;
    [SerializeField] private float speed = 5f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.position += transform.up *Time.deltaTime* speed;
            //rb.AddForce(transform.up * force);
        }
    }
}
