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
    public string statusPengetahuan;
    public string statusSikap;
    public string statusTindakan;

    public class PreTestPengetahuan
    {
        public string[] Pengetahuan;
    }
    public class PreTestSikap
    {
        public string[] Sikap;
    }
    public class PreTestTindakan
    {
        public string[] Tindakan;
    }

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

    public void SaveTest(string test, string json)
    {
        if (test == "Pengetahuan")
        {
            string _path = Application.persistentDataPath + "/saveTestPengetahuan.json";
            File.WriteAllText(_path, json);
        }else if (test == "Sikap")
        {
            string _path = Application.persistentDataPath + "/saveTestSikap.json";
            File.WriteAllText(_path, json);
        }else if (test == "Tindakan")
        {
            string _path = Application.persistentDataPath + "/saveTestTindakan.json";
            File.WriteAllText(_path, json);
        }
    }

    public bool HasFile(string test)
    {
        if (test == "Pengetahuan")
        {
            string _path = Application.persistentDataPath + "/saveTestPengetahuan.json";
            if (File.Exists(_path))
            {
                return true;
            }
        }else if (test == "Sikap")
        {
            string _path = Application.persistentDataPath + "/saveTestSikap.json";
            if (File.Exists(_path))
            {
                return true;
            }
        }else if (test == "Tindakan")
        {
            string _path = Application.persistentDataPath + "/saveTestTindakan.json";
            if (File.Exists(_path))
            {
                return true;
            }
        }
        return false;
    }
}
