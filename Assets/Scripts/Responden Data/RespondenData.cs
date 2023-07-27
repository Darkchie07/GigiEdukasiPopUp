using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.IO;
using UnityEditor;
using System.Net;

public class RespondenData : MonoBehaviour
{

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
        public string status;// 0 = still in, 1 = already logout
        public string PreTest;// 0 = belum, 1 = sudah;
        public string statusDebris; // 0 belum membuat, 1 = sudah membuat debris
        public Responden()
        {
            status = "0";
            PreTest = "0";
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

        // Set up the certificate handler to accept all certificates
        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

        CheckingDataFirst();
        doneLoadData = true;
        isDicari = false;
    }

    void CheckingDataFirst()
    {
        LoadFile();
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

    }

    public void RemoveDataGigi()
    {
        string _gigiPath = Application.persistentDataPath + "/savegambar.json";
        if (File.Exists(_gigiPath))
        {
            File.Delete(_gigiPath);
        }
    }

    public void RemoveDataTest()
    {
        string _skorPengetahuan = Application.persistentDataPath + "/saveTestPengetahuan.json";
        if (File.Exists(_skorPengetahuan))
        {
            File.Delete(_skorPengetahuan);
        }

        string _skorTestSikap = Application.persistentDataPath + "/saveTestSikap.json";
        if (File.Exists(_skorTestSikap))
        {
            File.Delete(_skorTestSikap);
        }

        string _skorTestTindakan = Application.persistentDataPath + "/saveTestTindakan.json";
        if (File.Exists(_skorTestTindakan))
        {
            File.Delete(_skorTestTindakan);
        }
    }

    public void RemoveDataKontrol()
    {
        string _fotoKontrol = Application.persistentDataPath + "/saveFotoKontrol.json";
        if (File.Exists(_fotoKontrol))
        {
            File.Delete(_fotoKontrol);
        }

        string _jawabanDebris = Application.persistentDataPath + "/saveDebris.json";
        if (File.Exists(_jawabanDebris))
        {
            File.Delete(_jawabanDebris);
        }

        string _skorKontrol = Application.persistentDataPath + "/saveSkorKontrol.json";
        if (File.Exists(_skorKontrol))
        {
            File.Delete(_skorKontrol);
        }
    }

    public void RemoveFotoTindakan()
    {
        string _fotoTindakan = Application.persistentDataPath + "/saveFotoTindakan.json";
        if (File.Exists(_fotoTindakan))
        {
            File.Delete(_fotoTindakan);
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