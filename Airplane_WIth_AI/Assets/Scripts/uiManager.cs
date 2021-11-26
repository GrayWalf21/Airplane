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
    }

    public void SetText(int index ,string st)
    {
        if(index == 0)
            speed.SetText("Speed: " + st);
        else if (index == 1)
            height.SetText("Height: " + st);
        else if (index == 2)
            power.SetText("Power: " + st);
    }
}
