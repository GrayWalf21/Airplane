using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    public static FileManager Instance;

    public Sample sample = new Sample();

    [HideInInspector] public bool first = true;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //print(contacts.transform.childCount+"asdasd asdasd asd as");
        sample = new Sample();
    }
    public void SavePlayerData()
    {
        SaveSystem.SaveData(sample);
        print("Saved");
    }
    public void LoadPlayerData()
    {
        Sample data = SaveSystem.Loadata();

        sample = data;
        print("Load");
    }
}
