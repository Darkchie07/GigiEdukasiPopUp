using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Jawaban : MonoBehaviour
{
    public int idx;
    public static Jawaban Instance { get; set; }
    public List<string> jawabanPengetahuan;
    public List<string> jawabanSikap;
    public List<string> jawabanTindakan;
    public string skorPengetahuan;
    public string skorSikap;
    public string skorTindakan;
    public List<string> listFotoTindakan = new List<string>();
    public List<string> skorResponden;
    public List<string> skorTest = new List<string>(3);

    void Awake()
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
    }

    void Start()
    {
        
    }

    public void UploadDataToDrive()
    {
        //API FORM 
        string _pengetahuan1 = jawabanPengetahuan[0];
        string _pengetahuan2 = jawabanPengetahuan[1];
        string _pengetahuan3 = jawabanPengetahuan[2];
        string _pengetahuan4 = jawabanPengetahuan[3];
        string _pengetahuan5 = jawabanPengetahuan[4];
        string _pengetahuan6 = jawabanPengetahuan[5];
        string _pengetahuan7 = jawabanPengetahuan[6];
        string _pengetahuan8 = jawabanPengetahuan[7];
        string _pengetahuan9 = jawabanPengetahuan[8];
        string _pengetahuan10 = jawabanPengetahuan[9];
        string _pengetahuan11 = jawabanPengetahuan[10];
        string _pengetahuan12 = jawabanPengetahuan[11];
        string _sikap1 = (jawabanSikap[0] == "0") ? "Tidak" : "Ya";
        string _sikap2 = (jawabanSikap[1] == "0") ? "Tidak" : "Ya";
        string _sikap3 = (jawabanSikap[2] == "0") ? "Tidak" : "Ya";
        string _sikap4 = (jawabanSikap[3] == "0") ? "Tidak" : "Ya";
        string _sikap5 = (jawabanSikap[4] == "0") ? "Tidak" : "Ya";
        string _sikap6 = (jawabanSikap[5] == "0") ? "Tidak" : "Ya";
        string _sikap7 = (jawabanSikap[6] == "0") ? "Tidak" : "Ya";
        string _sikap8 = (jawabanSikap[7] == "0") ? "Tidak" : "Ya";
        string _tindakan1 = (jawabanTindakan[0] == "0") ? "Tidak" : "Ya";
        string _tindakan2 = (jawabanTindakan[1] == "0") ? "Tidak" : "Ya";
        string _tindakan3 = (jawabanTindakan[2] == "0") ? "Tidak" : "Ya";
        string _tindakan4 = (jawabanTindakan[3] == "0") ? "Tidak" : "Ya";
        string _tindakan5 = (jawabanTindakan[4] == "0") ? "Tidak" : "Ya";
        string _tindakan6 = (jawabanTindakan[5] == "0") ? "Tidak" : "Ya";
        string _tindakan7 = (jawabanTindakan[6] == "0") ? "Tidak" : "Ya";
        string _tindakan8 = (jawabanTindakan[7] == "0") ? "Tidak" : "Ya";
        string _tindakan9 = (jawabanTindakan[8] == "0") ? "Tidak" : "Ya";
        string _tindakan10 = (jawabanTindakan[9] == "0") ? "Tidak" : "Ya";
        string _tindakan11 = (jawabanTindakan[10] == "0") ? "Tidak" : "Ya";
        string _tindakan12 = (jawabanTindakan[11] == "0") ? "Tidak" : "Ya";
        string _tindakan13 = (jawabanTindakan[12] == "0") ? "Tidak" : "Ya";
        string _tindakan14 = (jawabanTindakan[13] == "0") ? "Tidak" : "Ya";
        string _tindakan15 = (jawabanTindakan[14] == "0") ? "Tidak" : "Ya";
        string _tindakan16 = (jawabanTindakan[15] == "0") ? "Tidak" : "Ya";
        string _tindakan17 = (jawabanTindakan[16] == "0") ? "Tidak" : "Ya";
        string _tindakan18 = (jawabanTindakan[17] == "0") ? "Tidak" : "Ya";
        string _tindakan19 = (jawabanTindakan[18] == "0") ? "Tidak" : "Ya";
        string _tindakan20 = (jawabanTindakan[19] == "0") ? "Tidak" : "Ya";
        string _tindakan21 = (jawabanTindakan[20] == "0") ? "Tidak" : "Ya";
        string _tindakan22 = (jawabanTindakan[21] == "0") ? "Tidak" : "Ya";
        string _tindakan23 = (jawabanTindakan[22] == "0") ? "Tidak" : "Ya";
        string _tindakan24 = (jawabanTindakan[23] == "0") ? "Tidak" : "Ya";
        string _tindakan25 = (jawabanTindakan[24] == "0") ? "Tidak" : "Ya";
        string _tindakan26 = (jawabanTindakan[25] == "0") ? "Tidak" : "Ya";
        string _tindakan27 = (jawabanTindakan[26] == "0") ? "Tidak" : "Ya";
        string _tindakan28 = (jawabanTindakan[27] == "0") ? "Tidak" : "Ya";
        string _tindakan29 = (jawabanTindakan[28] == "0") ? "Tidak" : "Ya";
        string _tindakan30 = (jawabanTindakan[29] == "0") ? "Tidak" : "Ya";
        string _tindakan31 = (jawabanTindakan[30] == "0") ? "Tidak" : "Ya";
        string _tindakan32 = (jawabanTindakan[31] == "0") ? "Tidak" : "Ya";
        string _skorPengetahuan = skorPengetahuan;
        string _skorSikap = skorSikap;
        string _skorTindakan = skorTindakan;

        idx = 0;
        StartCoroutine(Helper.CoroutineUploadFormTest(
            _pengetahuan1, _pengetahuan2, _pengetahuan3, _pengetahuan4, _pengetahuan5, _pengetahuan6, _pengetahuan7, _pengetahuan8, _pengetahuan9, _pengetahuan10,
            _pengetahuan11, _pengetahuan12, _sikap1, _sikap2, _sikap3, _sikap4, _sikap5, _sikap6, _sikap7, _sikap8, _tindakan1, _tindakan2, _tindakan3, _tindakan4, _tindakan5,
            _tindakan6, _tindakan7, _tindakan8, _tindakan9, _tindakan10, _tindakan11, _tindakan12, _tindakan13, _tindakan14, _tindakan15, _tindakan16, _tindakan17, _tindakan18,
            _tindakan19, _tindakan20, _tindakan21, _tindakan22, _tindakan23, _tindakan24, _tindakan25, _tindakan26, _tindakan27, _tindakan28, _tindakan29, _tindakan30, _tindakan31, _tindakan32
            , _skorPengetahuan, _skorSikap, _skorTindakan, SuccessUploadFormRespondenTest, ErrorUploadFileResponden
        ));
    }

    void SuccessUploadFormRespondenTest()
    {
        listFotoTindakan = Data.Instance.dataFoto.foto.PathFoto.ToList();
        Debug.Log(listFotoTindakan.Count);
        for (int i = 0; i < listFotoTindakan.Count; i++)
        {
            Debug.Log("foto" + i);
            Helper.UploadImageTindakanResponden((file) => { _onDoneAction(); }, i);
        }
    }

    private void _onDoneAction()
    {
        idx += 1;
        if (idx == 32)
        {
            RespondenData.Instance.RemoveFotoTindakan();
            RespondenData.Instance.RemoveDataTest();
            TestScript.Instance.Done = true;
            TestScript.Instance.PopUpMessage("Data berhasil di upload");
            Debug.Log("Selesai");
        }
    }

    void ErrorUploadFileResponden()
    {
        // CloseLoading();
        Debug.Log("isi kuota deck");
    }
}
