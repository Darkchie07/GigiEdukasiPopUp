using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.IO;
using UnityEditor;

public class Data : MonoBehaviour
{
    public static Data Instance;

    public class Foto
    {
        public string[] StringFoto;
        public string[] PathFoto;
    }
    public class FotoFile
    {
        public Foto foto = new Foto();
    }

    public FotoFile dataFoto = new FotoFile();

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // LoadFile();
    }

    // public void LoadFile()
    // {
    //     string _testPath = Application.persistentDataPath + "/saveTest.json";
    //     if (File.Exists(_testPath))
    //     {
    //         string _data = File.ReadAllText(_testPath);
    //
    //         dataTest = JsonUtility.FromJson<TestFile>(_data);
    //     }
    //     else
    //     {
    //         dataTest = new TestFile();
    //     }
    // }

    public void SaveData(string data, string json)
    {
        if (data == "Pengetahuan")
        {
            string _path = Application.persistentDataPath + "/saveTestPengetahuan.json";
            File.WriteAllText(_path, json);
        }
        else if (data == "Sikap")
        {
            string _path = Application.persistentDataPath + "/saveTestSikap.json";
            File.WriteAllText(_path, json);
        }
        else if (data == "Tindakan")
        {
            string _path = Application.persistentDataPath + "/saveTestTindakan.json";
            File.WriteAllText(_path, json);
        }
        else if (data == "FotoTindakan")
        {
            string _path = Application.persistentDataPath + "/saveFotoTindakan.json";
            Debug.Log(_path);
            File.WriteAllText(_path, json);
        }
        else if (data == "Kontrol")
        {
            string _path = Application.persistentDataPath + "/saveDebris.json";
            File.WriteAllText(_path, json);
        }
        else if (data == "FotoKontrol")
        {
            string _path = Application.persistentDataPath + "/saveFotoKontrol.json";
            File.WriteAllText(_path, json);
        }
        else if (data == "Skor")
        {
            string _path = Application.persistentDataPath + "/saveSkor.json";
            File.WriteAllText(_path, json);
        }
        else if (data == "SkorKontrol")
        {
            string _path = Application.persistentDataPath + "/saveSkorKontrol.json";
            File.WriteAllText(_path, json);
        }
    }

    public bool HasFile(string data)
    {
        if (data == "Pengetahuan")
        {
            string _path = Application.persistentDataPath + "/saveTestPengetahuan.json";
            if (File.Exists(_path))
            {
                return true;
            }
        }
        else if (data == "Sikap")
        {
            string _path = Application.persistentDataPath + "/saveTestSikap.json";
            if (File.Exists(_path))
            {
                return true;
            }
        }
        else if (data == "Tindakan")
        {
            string _path = Application.persistentDataPath + "/saveTestTindakan.json";
            if (File.Exists(_path))
            {
                return true;
            }
        }
        else if (data == "FotoTindakan")
        {
            string _path = Application.persistentDataPath + "/saveFotoTindakan.json";
            if (File.Exists(_path))
            {
                return true;
            }
        }
        else if (data == "Kontrol")
        {
            string _path = Application.persistentDataPath + "/saveDebris.json";
            if (File.Exists(_path))
            {
                return true;
            }
        }
        else if (data == "FotoKontrol")
        {
            string _path = Application.persistentDataPath + "/saveFotoKontrol.json";
            if (File.Exists(_path))
            {
                return true;
            }
        }
        else if (data == "Skor")
        {
            string _path = Application.persistentDataPath + "/saveSkor.json";
            if (File.Exists(_path))
            {
                return true;
            }
        }
        else if (data == "SkorKontrol")
        {
            string _path = Application.persistentDataPath + "/saveSkorKontrol.json";
            if (File.Exists(_path))
            {
                return true;
            }
        }
        return false;
    }
}
