using System.IO;
using UnityEngine;

public class SaveManager : SingletonBehaviour<SaveManager>
{
    [SerializeField]
    private string SaveName;

    //Target path for the saving file
    private string SaveFilePath => Application.dataPath + $"/{SaveName}.json";

    private GameManager GameManager;

    private SaveData saveData;
    public SaveData SaveData
    {
        get
        {
            //If null try load from the disk
            if (saveData == null)
                saveData = Load();

            //If couldn't get load, create new one
            if (saveData == null)
                saveData = new SaveData();
 
            //Init dynamic settings from game settings
            if(saveData.DynamicSettings == null)
            {
                saveData.DynamicSettings = new DynamicSettings()
                {
                    MinSpinDistance = GameManager.GameSettings.MinSpinDistance,
                    MaxSpinDistance = GameManager.GameSettings.MaxSpinDistance,
                    ReelActivationDelay = GameManager.GameSettings.ReelActivationDelay,
                    SpinDuration = GameManager.GameSettings.SpinDuration
                };
            }
            return saveData;
        }
    }
 
    protected override bool Init()
    {
        if (base.Init()) return true;

        GameManager = GameManager.Instance as GameManager;
        return true;
    }

    //Load save object into target json file
    public void Save()
    {
        string Json = JsonUtility.ToJson(SaveData, true);
        File.WriteAllText(SaveFilePath, Json);
    }

    //Loads json file into a save object
    private SaveData Load()
    {
        if (File.Exists(SaveFilePath))
        {
            string Json = File.ReadAllText(SaveFilePath);
            SaveData Data = JsonUtility.FromJson<SaveData>(Json);
            return Data;
        }
        return null;
    }

    public void DeleteSave()
    {
        if (File.Exists(SaveFilePath))
        {
            File.Delete(SaveFilePath);
        }
    }
}
