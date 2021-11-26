using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class uiManager : MonoBehaviour
{
    public static uiManager Instance;

    private TextMeshProUGUI textMeshPro;

    private void OnEnable()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    void Start()
    {
        textMeshPro = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string st)
    {
        textMeshPro.SetText(st);
    }
}
