using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[System.Serializable]
public class GameData {

    public bool[] levelsCompleted = {false, false, false, false, false, false, false, false, false};
    public bool[] scrollsFound = {false, false, false, false, false, false, false, false};
    public bool tutorialDone = false;

    private static GameData instance = null;

    public static GameData Get() {
        if (instance == null) {
            Load();
        }
        return instance;
    }

    public static void Save() {
        if (instance == null) return;
        if (!Directory.Exists(Application.persistentDataPath + "/")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/");
        }
        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        FileStream stream = new FileStream(Application.persistentDataPath + "/GameData.xml", FileMode.Create);
        serializer.Serialize(stream, instance);
        stream.Close();
    }

    public static void Reset() {
        instance = new GameData();
        Save();
    }

    public static void Load() {
        if (File.Exists(Application.persistentDataPath + "/GameData.xml")) {
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            FileStream stream = new FileStream(Application.persistentDataPath + "/GameData.xml", FileMode.Open);
            instance = serializer.Deserialize(stream) as GameData;
            stream.Close();
        } else {
            instance = new GameData();
        }
    }
}