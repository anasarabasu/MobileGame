using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DataManager: MonoBehaviour {
    public static DataManager instance {get; private set;}
    private DataRoot data;
    private List<ISaveLoad> ISaveLoadList;
    private string dataPath;

    private void Awake() {
        instance = this;
        data = new DataRoot() {
            userData = new UserData()
        };
        ISaveLoadList = FindSaveLoadDataList();
        dataPath =  Application.persistentDataPath + Path.AltDirectorySeparatorChar + "GameData.json";

        if(File.Exists(dataPath)) {
            LoadFile();
        }
        else {
            string createJSON = JsonUtility.ToJson(data, true); //create default json file
            using (StreamWriter streamWriter = new StreamWriter(dataPath))
                streamWriter.Write(createJSON);
            
            Debug.Log("Save file created");
        }
    }

    private List<ISaveLoad> FindSaveLoadDataList() {
        IEnumerable<ISaveLoad> obj = FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>();
        return new List<ISaveLoad>(obj);
    }

    public void SaveFile() {
        foreach(ISaveLoad gameObject in ISaveLoadList) 
            gameObject.Save(ref data);

        string saveJSON = JsonUtility.ToJson(data, true); //write data to json
        using (StreamWriter streamWriter = new StreamWriter(dataPath))
            streamWriter.Write(saveJSON);

        Debug.Log("File saved");
    }

    public void LoadFile() {
        string loadJSON;
        using (StreamReader streamReader = new StreamReader(dataPath))
            loadJSON = streamReader.ReadToEnd();
        DataRoot loadData = JsonUtility.FromJson<DataRoot>(loadJSON); //create data from json
        data = loadData;

        foreach(ISaveLoad gameObject in ISaveLoadList) 
            gameObject.Load(data);

        Debug.Log("Save file loaded");
    }

    public void DeleteFile() {
        if(File.Exists(dataPath)) {
            File.Delete(dataPath);
            SceneManager.LoadScene("MainMenu");
            Debug.Log("Save file deleted... Game Reloaded");
        }
        else {
            Debug.Log("No save file to delete");
        }
    }

    public void DeleteJSON(InputAction.CallbackContext context) {
        DeleteFile();
    }

    public void DisplayJSON(InputAction.CallbackContext context) {
        Debug.Log(JsonUtility.ToJson(data, true));
    }

    private void OnApplicationQuit() {
    }

    private void OnApplicationPause(bool pauseStatus) {
    //    SaveFile();
    }
}
