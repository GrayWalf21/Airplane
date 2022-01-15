using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    public static FileManager Instance;

    public Sample sample = new Sample();

    [HideInInspector] public bool first = true;
    [HideInInspector] public int index = 0;
    [HideInInspector] public string[] output ;

    private Airplane airplane;

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
        LoadPlayerData();
    }
    private void Start()
    {
        airplane = FindObjectOfType<Airplane>();
         if (PlayerPrefs.HasKey("Index"))
         {
             index = PlayerPrefs.GetInt("Index");
         }
         else
         {
             index = 0;
             PlayerPrefs.SetInt("Index", index);
         }
        //index = PlayerPrefs.GetInt("Index");
    }
    public void SavePlayerData()
    {
        print("F9 Pressed Saving the Data");
        SaveSystem.SaveData(sample);
        print("Saved");
        sample = new Sample();
        airplane.sample = new Sample();
        index++;
        PlayerPrefs.SetInt("Index",index);
        first = true;
    }
    public void LoadPlayerData()
    {
        output = SaveSystem.Loadata();

        print("Load");
    }
}
