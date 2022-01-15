using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private bool canWrite = false;
    private bool driveManually = true;
    [HideInInspector] public Airplane airplane;

    private void OnEnable()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    void Start()
    {
        airplane = FindObjectOfType<Airplane>();
    }
    private void Update()
    {
        var pw = Input.GetAxis("Power");
        var vt = Input.GetAxis("Vertical");
        var ht = Input.GetAxis("Horizontal");
        var pt = Input.GetAxis("Perpendicular");

        if (Input.GetKeyDown(KeyCode.Keypad0)) 
        {
            airplane.StopEngine();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            PlayerPrefs.SetInt("Index", 0);
            FileManager.Instance.index = 0;
            print("index = 0");
        }

        if (Input.GetKeyDown(KeyCode.T)) 
        {
            airplane.ClearEverything();
        }

        if (Input.GetKeyDown(KeyCode.F)) 
        {
            airplane.StartForce();
        }

        if (Input.GetKeyDown(KeyCode.F9)) 
        {
            FileManager.Instance.SavePlayerData();
        }

        if (Input.GetKeyDown(KeyCode.F7) && !canWrite)
        {
            canWrite = true;
            print("canWrite = true");
        }
        else if (Input.GetKeyDown(KeyCode.F7) && canWrite)
        {
            canWrite = false;
            print("canWrite = false");
        }

        if (Input.GetKeyDown(KeyCode.F2) && driveManually)
        {
            driveManually = false;
            airplane.driveManually = false;
            print("driveManually = false");
        }
        else if (Input.GetKeyDown(KeyCode.F2) && !driveManually)
        {
            driveManually = true;
            airplane.driveManually = true;
            print("driveManually = true");
        }

        if (driveManually)
        {
            airplane.pw = pw;
            airplane.vt = vt;
            airplane.ht = ht;
            airplane.pt = pt;
        }
    }
}