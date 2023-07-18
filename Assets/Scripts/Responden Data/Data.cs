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
    public class Pretest
    {
        public List<Test> listPreTest = new List<Test>();
    }

    public class Test
    {
        public string[] Pengetahuan;
        public string[] Sikap;
        public string[] Tindakan;
    }

    public class TestFile
    {
        public Pretest preTest = new Pretest();
    }

    public TestFile dataTest = new TestFile();

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
        LoadFile();
    }

    public void LoadFile()
    {
        string _testPath = Application.persistentDataPath + "/saveTest.json";
        if (File.Exists(_testPath))
        {
            string _data = File.ReadAllText(_testPath);

            dataTest = JsonUtility.FromJson<TestFile>(_data);
        }
        else
        {
            dataTest = new TestFile();
        }
    }

    public void SaveTest(string json)
    {
        string _path = Application.persistentDataPath + "/saveTest.json";
        File.WriteAllText(_path, json);
    }
}
