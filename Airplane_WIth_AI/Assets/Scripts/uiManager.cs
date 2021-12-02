using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class uiManager : MonoBehaviour
{
    public static uiManager Instance;

    private TextMeshProUGUI speed;
    private TextMeshProUGUI height;
    private TextMeshProUGUI power;
    private TextMeshProUGUI distance;

    private void OnEnable()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    void Start()
    {
        speed = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        height = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        power = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        distance = transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
    }

    public void SetText(int index ,string st)
    {
        switch (index)
        {
            case 0:
                speed.SetText("Speed: " + st);
                break;
            case 1:
                height.SetText("Height: " + st);
                break;
            case 2:
                power.SetText("Power: " + st);
                break;
            case 3:
                distance.SetText("Distance: " + st);
                break;
        }
     
    }
}
