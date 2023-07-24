using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEditor.MemoryProfiler;

public class RespondenData : MonoBehaviour
{
    #region CLASS DEBRIS DATA
    [System.Serializable]
    public class Debris
    {
        public List<DebrisData> listDebris = new List<DebrisData>();
    }

    [System.Serializable]
    public class DebrisData
    {
        public string namaGigi;
        public bool status;
        public string stringFoto;
        public string pathFoto;
    }
    [System.Serializable]
    public class DebrisFile
    {
        public Debris debris = new Debris();
    }

    #endregion

    #region CLASS DATA RESPONDEN
    [System.Serializable]
    public class DataResponden
    {
        public List<Responden> listResponden = new List<Responden>();
    }

    [System.Serializable]
    public class Responden
    {
        public string nama;
        public string umur;
        public string jenisKelamin;
        //public List<GambarGigi> daftarGambargigi = new List<GambarGigi>(7);
        public string status; // 0 = still in, 1 = already logout
        public string statusDebris; // 0 belum membuat, 1 = sudah membuat debris
        public Responden()
        {
            status = "0";
        }

        public void SetDataAwal(string _nama, string _umur, string _jenisKelamin)
        {
            nama = _nama;
            umur = _umur;
            jenisKelamin = _jenisKelamin;
        }

    }




    [System.Serializable]
    public class DataGambarGigi
    {
        public List<string> listImageGigiPagi = new List<string>();
        public List<string> listImageGigiMalam = new List<string>();
        public void SaveGambar(string _strImage, PageFotoScript.StatusFotoGigi _status)
        {
            if (_status == PageFotoScript.StatusFotoGigi.PAGI)
                listImageGigiPagi.Add(_strImage);
            else
                listImageGigiMalam.Add(_strImage);
            Instance.SaveGambarGigi();
        }
    }
    #endregion

    [Header("DATA GAMBAR GIGI")]
    public DataGambarGigi dataGambarGigi = new DataGambarGigi();

    [Header("DATA DEBRIS")]
    public DebrisFile dataDebris = new DebrisFile();

    [Header("DATA RESPONDEN")]
    public DataResponden dataResponden;

    [Header("CURRENT DATA SELECTED")]
    public Responden currentDataSelected;

    public static RespondenData Instance;

    public bool doneLoadData = false;

    public bool isDicari;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(this.gameObject);

        CheckingDataFirst();
        doneLoadData = true;
        isDicari = false;
    }

    void CheckingDataFirst()
    {
        LoadFile();
        Debug.Log(dataResponden.listResponden[dataResponden.listResponden.Count - 1].status == "1");
        if (dataResponden.listResponden.Count == 0) // jika kosong
        {
            print("data belum ada");
            currentDataSelected = new Responden();
        }
        else
        {
            //cek data terakhirnya
            if (dataResponden.listResponden[dataResponden.listResponden.Count - 1].status == "1") // sudah logout
            {
                currentDataSelected = new Responden();
            }
            else
            {
                currentDataSelected = dataResponden.listResponden[dataResponden.listResponden.Count - 1];
            }
        }
    }


    /// <summary>
    /// fungsi untuk convert data responden ke string json
    /// </summary>
    /// <returns></returns>
    public string ConvertDataToJson()
    {
        string json = JsonUtility.ToJson(dataResponden);
        return json;
    }

    /// <summary>
    /// fungsi untuk convert data gambar gigi ke string json
    /// </summary>
    /// <returns></returns>
    public string ConvertDataGambarToJson()
    {
        string json = JsonUtility.ToJson(dataGambarGigi);
        return json;
    }

    /// <summary>
    /// fungsi untuk melakukan save file
    /// </summary>
    public void SaveDataResponden()
    {
        string _path = Application.persistentDataPath + "/savefile.json";
        File.WriteAllText(_path, ConvertDataToJson());
    }

    /// <summary>
    /// fungsi untuk save gambar gigi
    /// </summary>
    public void SaveGambarGigi()
    {
        string _gigiPath = Application.persistentDataPath + "/savegambar.json";
        File.WriteAllText(_gigiPath, ConvertDataGambarToJson());
    }


    /// <summary>
    /// fungsi untuk save debris
    /// </summary>
    /// <param name="json">string json nya</param>
    public void SaveDebris(string json)
    {
        //buat status responden debris nya jadi 1
        currentDataSelected.statusDebris = "1";

        string _path = Application.persistentDataPath + "/saveDebris.json";
        File.WriteAllText(_path, json);
        SaveDataResponden();
    }


    /// <summary>
    /// fungsi untuk melakukan update file csv
    /// </summary>
    public void CreateSaveCsvFile()
    {
        string _fileName = Application.persistentDataPath + "/dataSiswa.csv";
        if (File.Exists(_fileName))
        {
            if (CheckData($"{currentDataSelected.nama},{currentDataSelected.umur}," +
                $"{(currentDataSelected.jenisKelamin == "0" ? "Laki-laki" : "Perempuan")}")) //jika ada data kembar, return
            {
                return;
            }
        }
        WriteDataCsv(_fileName);
    }

    /// <summary>
    /// save data csv
    /// </summary>
    /// <param name="_fileName"></param>
    void WriteDataCsv(string _fileName)
    {
        TextWriter tw = new StreamWriter(_fileName, false);
        tw.WriteLine("Nama, Umur, Jenis Kelamin");
        tw.Close();

        tw = new StreamWriter(_fileName, true);

        foreach (var a in dataResponden.listResponden)
        {
            tw.WriteLine($"{a.nama},{a.umur},{(a.jenisKelamin == "0" ? "Laki-laki" : "Perempuan")}");
        }
        tw.Close();

    }

    /// <summary>
    /// check same data
    /// </summary>
    /// <param name="namaCari">data yg dicari</param>
    /// <returns></returns>
    public bool CheckData(string namaCari)
    {
        string _fileName = Application.persistentDataPath + "/dataSiswa.csv";
        StreamReader strReader = new StreamReader(_fileName);
        bool endOfFile = false;
        bool sameData = false;
        while (!endOfFile)
        {
            string data_string = strReader.ReadLine();
            if (data_string == null)
            {
                endOfFile = true;
                break;
            }

            var data_values = data_string.Split(',');
            for (int i = 0; i < data_values.Length; i++)
            {
                Debug.Log(data_values[i]);
            }
            string dataSama = data_values[0].ToString() + "," + data_values[1].ToString() + "," +
                              data_values[2].ToString() + "," + data_values[3].ToString();
            if (dataSama == namaCari)
            {
                sameData = true;
                break;
            }
        }
        strReader.Close();

        return sameData;
    }


    /// <summary>
    /// create new data responden
    /// </summary>
    public void InsertNewDataResponden()
    {
        dataResponden.listResponden.Add(currentDataSelected);
        SaveDataResponden();
    }


    /// <summary>
    /// Fungsi untuk melakukan load file
    /// </summary>
    [ContextMenu("LOAD DATA")]
    public void LoadFile()
    {
        //load data responden
        string _path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(_path))
        {
            string _data = File.ReadAllText(_path);

            dataResponden = JsonUtility.FromJson<DataResponden>(_data);
        }
        else
        {
            dataResponden = new DataResponden();
        }

        //load gambar gigi
        string _gigiPath = Application.persistentDataPath + "/savegambar.json";
        if (File.Exists(_gigiPath))
        {
            string _data = File.ReadAllText(_gigiPath);

            dataGambarGigi = JsonUtility.FromJson<DataGambarGigi>(_data);
        }
        else
        {
            dataGambarGigi = new DataGambarGigi();
        }

        //load debris
        string _debrisPath = Application.persistentDataPath + "/saveBomba.json";
        if (File.Exists(_debrisPath))
        {
            string _data = File.ReadAllText(_debrisPath);

            dataDebris = JsonUtility.FromJson<DebrisFile>(_data);
        }
        else
        {
            dataDebris = new DebrisFile();
        }
    }

    public void RemoveDataGigi()
    {
        string _gigiPath = Application.persistentDataPath + "/savegambar.json";
        if (File.Exists(_gigiPath))
        {
            File.Delete(_gigiPath);
        }
    }

    public void RemoveDebris()
    {
        string _path = Application.persistentDataPath + "/saveDebris.json";
        if (File.Exists(_path))
        {
            File.Delete(_path);
        }
    }

}