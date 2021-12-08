using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float force = 20f;
    [SerializeField] private float speed = 5f;
    float time;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.zero, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.position += transform.up *Time.deltaTime* speed;
            //rb.AddForce(transform.up * force);
        }
        if (Input.GetMouseButtonDown(1))
        {
            rb.useGravity = true;
            time = Time.time;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        print(Time.time - time);
    }
}
