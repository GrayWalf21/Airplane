using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Little_Airport : MonoBehaviour
{
    [SerializeField] GameObject airplane;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(airplane.transform.position.x, airplane.transform.position.y + 2f, airplane.transform.position.z + 8.16f);
    }
}
