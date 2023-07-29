using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Debris : MonoBehaviour
{
	private int Index;
	public static Debris Instance { get; set; }
	public List<string> jawabanKontrol;
	public List<int> skorKontrol;
	public List<string> listFotoKontrol = new List<string>();
	public List<int> skorKontrolResponden;
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
		string _skorgrahamKananAtas = skorKontrol[0].ToString();
		string _skordepanAtas = skorKontrol[1].ToString();
		string _skorgrahamKiriAtas = skorKontrol[2].ToString();
		string _skorgrahamKananBawah = skorKontrol[3].ToString();
		string _skordepanBawah = skorKontrol[4].ToString();
		string _skorgrahamKiriBawah = skorKontrol[5].ToString();
		string _skor = skorKontrol[6].ToString();

		TestScript.Instance.DoneButton.gameObject.SetActive(false);
		TestScript.Instance.PopUpMessage("Proses Upload..");
		Index = 0;
		//API FORM
		StartCoroutine(Helper.CoroutineUploadFormGigiResponden(
		_grahamKananAtas, _depanAtas, _grahamKiriAtas, _grahamKiriBawah, _depanBawah, _grahamKananBawah, _skorgrahamKananAtas, _skordepanAtas, _skorgrahamKiriAtas,
		_skorgrahamKananBawah, _skordepanBawah, _skorgrahamKiriBawah, _skor, SuccessUploadFormRespondenDebris, ErrorUploadFileResponden
	   ));
	}

	void SuccessUploadFormRespondenDebris()
	{
		listFotoKontrol = Data.Instance.dataFoto.foto.PathFoto.ToList();
		Debug.Log(listFotoKontrol.Count);
		for (int i = 0; i < listFotoKontrol.Count; i++)
		{
			Helper.UploadImageKontrolResponden((file) => { _onDoneAction(); }, i);
		}
	}

	private void _onDoneAction()
	{
		Index += 1;
		if (Index == 6)
		{
			TestScript.Instance.SaveSkor(TestScript.Instance.listJawabanYa.Count);
			TestScript.Instance.SaveSkor(TestScript.Instance.skor);
			TestScript.Instance.SaveSkorKontrol();
			RespondenData.Instance.RemoveDataKontrol();
			TestScript.Instance.Done = true;
			TestScript.Instance.PopUpMessage("Data Berhasil diUpload");
			TestScript.Instance.DoneButton.gameObject.SetActive(true);
		}
		Debug.Log("Selesai");
	}

	void ErrorUploadFileResponden()
	{
		TestScript.Instance.PopUpMessage("Data Gagal diUpload");
	}
}
