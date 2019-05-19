using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public struct LevelProgress {
    public bool isUnlocked;
}

public class ProgressManager : MonoBehaviour {
    public static ProgressManager instance;
    
    public LevelProgress[] levels;

	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);

        DontDestroyOnLoad(this.gameObject);
	}

    void Start()
    {
        LoadData();
    }
	

	public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("pts.sav");

        formatter.Serialize(saveFile, levels);
        saveFile.Close();
    }

    public void LoadData()
    {
        if (!File.Exists("pts.sav"))
            return;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Open("pts.sav", FileMode.Open);

        levels = (LevelProgress[])bf.Deserialize(saveFile);

        saveFile.Close();
    }
}
