using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Debris : MonoBehaviour
{
    public static Debris Instance { get; set; }
    public List<string> jawabanKontrol;
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
    public void UploadDebrisToDrive()
    {
        string _grahamKananAtas = (jawabanKontrol[0] == "1") ? "Ada" : "Tidak ada";
        string _depanAtas = (jawabanKontrol[1] == "1") ? "Ada" : "Tidak ada";
        string _grahamKiriAtas = (jawabanKontrol[2] == "1") ? "Ada" : "Tidak ada";
        string _grahamKiriBawah = (jawabanKontrol[3] == "1") ? "Ada" : "Tidak ada";
        string _depanBawah = (jawabanKontrol[4] == "1") ? "Ada" : "Tidak ada";
        string _grahamKananBawah = (jawabanKontrol[5] == "1") ? "Ada" : "Tidak ada";
        string _skor = jawabanKontrol[6];

        //API FORM
        StartCoroutine(Helper.CoroutineUploadFormGigiResponden(
        _grahamKananAtas, _depanAtas, _grahamKiriAtas, _grahamKiriBawah, _depanBawah, _grahamKananBawah, _skor
        , SuccessUploadFormRespondenDebris, ErrorUploadFileResponden
       ));
    }

    void SuccessUploadFormRespondenDebris()
    {
        for (int i = 0; i < TestScript.Instance.ListpathFoto.Count; i++)
        {
            Helper.UploadImageTindakanResponden((file) => { _onDoneAction(); }, i);
        }
    }

    private void _onDoneAction()
    {
        Debug.Log("Selesai");
    }

    void ErrorUploadFileResponden()
    {
        // CloseLoading();
        Debug.Log("isi kuota deck");
    }
}
