using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class SaveTool
{
    public static List<SaveData> savedGames = new List<SaveData>();
    const string saveFileName = "/savedGames.gd";

    //it's static so we can call it from anywhere
    public static void Save()
    {
        SaveTool.savedGames.Add(SaveData.current);
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + saveFileName); //you can call it anything you want
        bf.Serialize(file, SaveTool.savedGames);
        file.Close();
        Debug.Log("Saved " + Application.persistentDataPath + saveFileName);
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + saveFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveFileName, FileMode.Open);
            SaveTool.savedGames = (List<SaveData>)bf.Deserialize(file);
            SaveData.current = SaveTool.savedGames.First();
            file.Close();
            Debug.Log("Loaded " + Application.persistentDataPath + saveFileName);
        }
        else
            Debug.Log("Failed to load " + Application.persistentDataPath + saveFileName);
    }
}

[System.Serializable]
public class SaveData
{
    public static SaveData current;

    public List<LevelID> UnlockedLevels;

    public static void VictoryUnlock(LevelID id)
    {
        if (!current.UnlockedLevels.Contains(id))
            current.UnlockedLevels.Add(id);
    }

   /* public static void Initialize()
    {
        current = new SaveData();
    }*/

    public SaveData()
    {
        UnlockedLevels = new List<LevelID>();
    }
}

