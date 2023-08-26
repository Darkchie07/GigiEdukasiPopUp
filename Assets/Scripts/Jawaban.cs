using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
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
		string _skorPengetahuan = skorPengetahuan;
		string _skorSikap = skorSikap;
		string _skorTindakan = skorTindakan;
		TestScript.Instance.DoneButton.gameObject.SetActive(false);
		TestScript.Instance.PopUpMessage("Proses Upload..");
		idx = 0;
		StartCoroutine(Helper.CoroutineUploadFormTest(
			_pengetahuan1, _pengetahuan2, _pengetahuan3, _pengetahuan4, _pengetahuan5, _pengetahuan6, _pengetahuan7, _pengetahuan8, _pengetahuan9, _pengetahuan10,
			_pengetahuan11, _pengetahuan12, _sikap1, _sikap2, _sikap3, _sikap4, _sikap5, _sikap6, _sikap7, _sikap8, _tindakan1, _tindakan2, _tindakan3, _tindakan4, _tindakan5,
			_tindakan6, _tindakan7, _tindakan8, _tindakan9, _tindakan10, _skorPengetahuan, _skorSikap, _skorTindakan, SuccessUploadFormRespondenTest, ErrorUploadFileResponden
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
		if (idx == 10)
		{
			TestScript.Instance.SaveSkor(0);
            RespondenData.Instance.currentDataSelected.PreTest = "1";
            RespondenData.Instance.SaveDataResponden();
			RespondenData.Instance.RemoveFotoTindakan();
			RespondenData.Instance.RemoveDataTest();
			TestScript.Instance.Done = true;
			TestScript.Instance.PopUpMessage("Data berhasil diUpload");
			TestScript.Instance.DoneButton.gameObject.SetActive(true);
			Debug.Log("Selesai");
		}
	}

	void ErrorUploadFileResponden()
	{
		TestScript.Instance.PopUpMessage("Data Gagal diUpload");
	}
}
