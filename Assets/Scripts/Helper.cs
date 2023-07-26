using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.IO;
using UnityGoogleDrive;

public static class Helper
{

	/// <summary>
	/// convert texture2d ke sprite
	/// </summary>
	/// <param name="_tex"></param>
	/// <returns></returns>
	public static Sprite TextureToSprite(Texture2D _tex)
	{
		Sprite newSprite = Sprite.Create(_tex, new Rect(0.0f, 0.0f, _tex.width, _tex.height), new Vector2(0.5f, 0.5f), 100.0f);

		return newSprite;
	}


	/// <summary>
	/// convert texture2d menjadi base64 string
	/// </summary>
	/// <param name="tex"></param>
	/// <returns></returns>
	public static string TextureToBase64(Texture2D tex)
	{
		if (tex == null)
			return null;

		byte[] pngByte = duplicateTexture(tex).EncodeToPNG();
		string b64 = Convert.ToBase64String(pngByte);
		return b64;
	}

	/// <summary>
	/// convert b64 string jadi texture
	/// </summary>
	/// <param name="base64"></param>
	/// <returns></returns>
	public static Texture2D Base64ToTexture(string base64)
	{
		if (string.IsNullOrEmpty(base64))
			return null;



		byte[] pngByte = Convert.FromBase64String(base64);

		Texture2D tex = new Texture2D(128, 128);
		tex.LoadImage(pngByte);

		return tex;
	}

	public static Texture2D duplicateTexture(Texture2D source)
	{
		RenderTexture renderTex = RenderTexture.GetTemporary(
					source.width,
					source.height,
					0,
					RenderTextureFormat.Default,
					RenderTextureReadWrite.Linear);

		Graphics.Blit(source, renderTex);
		RenderTexture previous = RenderTexture.active;
		RenderTexture.active = renderTex;
		Texture2D readableText = new Texture2D(source.width, source.height);
		readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
		readableText.Apply();
		RenderTexture.active = previous;
		RenderTexture.ReleaseTemporary(renderTex);
		return readableText;
	}

	/// <summary>
	/// LOGOUT SYSTEM
	/// </summary>
	public static void LogOut()
	{
		RespondenData.Instance.currentDataSelected.status = "1";
		RespondenData.Instance.SaveDataResponden();
		RespondenData.Instance.RemoveDataGigi();
		RespondenData.Instance.RemoveDebris();
		Debug.Log("logout");
	}


	#region CHANGE SCENE
	/// <summary>
	/// change scene
	/// </summary>
	/// <param name="_targetSceneName"></param>
	public static void ChangeScene(string _targetSceneName)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(_targetSceneName);
	}

	public static void GoToPemantauanSikatGigi()
	{
		ChangeScene("PemantauanSikatGigi");
	}

	public static void GoToHomeMenu()
	{
		ChangeScene("Scenes/Pengetahuan");
	}
	// public static void GoToMateri()
	// {
	//     if (!Audio.Instance.audioSource.isPlaying)
	//         Audio.Instance.PlayMusic();
	//     ChangeScene("Scenes/HomeMenu/HomeMenu 1");
	// }

	public static void GoToVideoMateri()
	{
		ChangeScene("Scenes/HomeMenu/VideoScene");
	}
	#endregion

	#region METHOD TO UPLOAD FORM RESPONDEN DATA
	private static string UrlFormRespondenData = "https://docs.google.com/forms/d/e/1FAIpQLSc_Hfnf6n4BCZvrl_UhE_qf3XGIGTnIsWo7yLMVJbXlqCZYtg/formResponse";
	private static string UrlFormRespondenDataDebris = "https://docs.google.com/forms/d/e/1FAIpQLSdwEdTuCAw0QkdWN1REdNxVW6Mi9F6RZ6GCleX25aCe5fVhWw/formResponse";
	private static string UrlFormRespondenDataPreTest = "https://docs.google.com/forms/d/e/1FAIpQLScC3sjO61fwPuDZlnV2pZfHzDoAqKjuCt1rVRxfgFYJRajp8w/formResponse";
	public static IEnumerator CoroutineUploadFormRespondenData(string _nama, string _umur, string _jnsKelamin, Action _success, Action _error)
	{
		WWWForm form = new WWWForm();
		form.AddField("entry.1368113041", _nama);
		form.AddField("entry.1222889340", _umur);
		form.AddField("entry.1561357289", _jnsKelamin);

		UnityWebRequest www = UnityWebRequest.Post(UrlFormRespondenData, form);


		yield return www.SendWebRequest();


		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.Log(www.error);
			_error();
			yield break;
		}

		_success();
		www.Dispose();
		yield break;

	}

	public static IEnumerator CoroutineUploadFormGigiResponden(string _grahamKananAtas, string _depanAtas, string _grahamKiriAtas, string _grahamKiriBawah, string _depanBawah, string _grahamKananBawah,
		string _skorgrahamKananAtas, string _skordepanAtas, string _skorgrahamKiriAtas, string _skorgrahamKiriBawah, string _skordepanBawah, string _skorgrahamKananBawah, string _skorTotal, Action _success, Action _error)
	{
		WWWForm form = new WWWForm();
		form.AddField("entry.1088436913", RespondenData.Instance.currentDataSelected.nama);
		form.AddField("entry.983827928", RespondenData.Instance.currentDataSelected.umur);
		form.AddField("entry.2108389131", (RespondenData.Instance.currentDataSelected.jenisKelamin == "0") ? "Laki-Laki" : "Perempuan");
		form.AddField("entry.1402146777", _grahamKananAtas);
		form.AddField("entry.1578178223", _depanAtas);
		form.AddField("entry.1196549055", _grahamKiriAtas);
		form.AddField("entry.1454885654", _grahamKiriBawah);
		form.AddField("entry.2075621295", _depanBawah);
		form.AddField("entry.1238623945", _grahamKananBawah);
		form.AddField("entry.50668979", _skorgrahamKananAtas);
		form.AddField("entry.1828658984", _skordepanAtas);
		form.AddField("entry.1073350272", _skorgrahamKiriAtas);
		form.AddField("entry.1234534435", _skorgrahamKiriBawah);
		form.AddField("entry.1789352277", _skordepanBawah);
		form.AddField("entry.1913966691", _skorgrahamKananBawah);
		form.AddField("entry.780479308", _skorTotal);

		UnityWebRequest www = UnityWebRequest.Post(UrlFormRespondenDataDebris, form);


		yield return www.SendWebRequest();


		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.Log(www.error);
			yield break;
		}

		_success();
		www.Dispose();
		yield break;
	}
	public static IEnumerator CoroutineUploadFormTest(string _pengetahuan1, string _pengetahuan2, string _pengetahuan3, string _pengetahuan4, string _pengetahuan5, string _pengetahuan6,
														string _pengetahuan7, string _pengetahuan8, string _pengetahuan9, string _pengetahuan10, string _pengetahuan11, string _pengetahuan12,
														string _sikap1, string _sikap2, string _sikap3, string _sikap4, string _sikap5, string _sikap6, string _sikap7, string _sikap8,
														string _tindakan1, string _tindakan2, string _tindakan3, string _tindakan4, string _tindakan5, string _tindakan6, string _tindakan7, string _tindakan8,
														string _tindakan9, string _tindakan10, string _tindakan11, string _tindakan12, string _tindakan13, string _tindakan14, string _tindakan15, string _tindakan16,
														string _tindakan17, string _tindakan18, string _tindakan19, string _tindakan20, string _tindakan21, string _tindakan22, string _tindakan23, string _tindakan24,
														string _tindakan25, string _tindakan26, string _tindakan27, string _tindakan28, string _tindakan29, string _tindakan30, string _tindakan31, string _tindakan32, string _skorPengetahuan, string _skorSikap, 
														string _skorTindakan, Action _success, Action _error)
	{
		WWWForm form = new WWWForm();

		#region EntryData
		form.AddField("entry.9485965", RespondenData.Instance.currentDataSelected.nama);
		form.AddField("entry.492007321", RespondenData.Instance.currentDataSelected.umur);
		form.AddField("entry.329954262", (RespondenData.Instance.currentDataSelected.jenisKelamin == "0") ? "Laki-Laki" : "Perempuan");
		form.AddField("entry.1379014718", _pengetahuan1);
		form.AddField("entry.1838805648", _pengetahuan2);
		form.AddField("entry.1395587743", _pengetahuan3);
		form.AddField("entry.1684285751", _pengetahuan4);
		form.AddField("entry.260675691", _pengetahuan5);
		form.AddField("entry.1733836360", _pengetahuan6);
		form.AddField("entry.734379896", _pengetahuan7);
		form.AddField("entry.1641036112", _pengetahuan8);
		form.AddField("entry.939010579", _pengetahuan9);
		form.AddField("entry.1977359474", _pengetahuan10);
		form.AddField("entry.54452414", _pengetahuan11);
		form.AddField("entry.477479094", _pengetahuan12);
		form.AddField("entry.1165789429", _sikap1);
		form.AddField("entry.861183063", _sikap2);
		form.AddField("entry.121420768", _sikap3);
		form.AddField("entry.1116960718", _sikap4);
		form.AddField("entry.424260268", _sikap5);
		form.AddField("entry.1299215184", _sikap6);
		form.AddField("entry.102795090", _sikap7);
		form.AddField("entry.363748346", _sikap8);
		form.AddField("entry.73393216", _tindakan1);
		form.AddField("entry.1213613168", _tindakan2);
		form.AddField("entry.116285953", _tindakan3);
		form.AddField("entry.332259448", _tindakan4);
		form.AddField("entry.2041953228", _tindakan5);
		form.AddField("entry.198581723", _tindakan6);
		form.AddField("entry.317831849", _tindakan7);
		form.AddField("entry.728273302", _tindakan8);
		form.AddField("entry.1091682822", _tindakan9);
		form.AddField("entry.1515630106", _tindakan10);
		form.AddField("entry.220797466", _tindakan11);
		form.AddField("entry.125281289", _tindakan12);
		form.AddField("entry.1643425419", _tindakan13);
		form.AddField("entry.391668054", _tindakan14);
		form.AddField("entry.2028582010", _tindakan15);
		form.AddField("entry.1871008435", _tindakan16);
		form.AddField("entry.412249163", _tindakan17);
		form.AddField("entry.364643505", _tindakan18);
		form.AddField("entry.1232366264", _tindakan19);
		form.AddField("entry.173037941", _tindakan20);
		form.AddField("entry.1284230937", _tindakan21);
		form.AddField("entry.1858128209", _tindakan22);
		form.AddField("entry.997858969", _tindakan23);
		form.AddField("entry.1422154499", _tindakan24);
		form.AddField("entry.1353601833", _tindakan25);
		form.AddField("entry.1320732202", _tindakan26);
		form.AddField("entry.1943190957", _tindakan27);
		form.AddField("entry.630684707", _tindakan28);
		form.AddField("entry.1163226300", _tindakan29);
		form.AddField("entry.703134618", _tindakan30);
		form.AddField("entry.1339464562", _tindakan31);
		form.AddField("entry.250074568", _tindakan32);
		form.AddField("entry.94854912", _skorPengetahuan);
		form.AddField("entry.46003329", _skorSikap);
		form.AddField("entry.274418371", _skorTindakan);
		#endregion

		UnityWebRequest www = UnityWebRequest.Post(UrlFormRespondenDataPreTest, form);


		yield return www.SendWebRequest();


		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.Log(www.error);
			yield break;
		}

		_success();
		www.Dispose();
		yield break;
	}

	#endregion

	#region DRIVE FUNCTION TO UPLOAD IMAGE

	public static string UserAccount = "fatikhamaulidiah@gmail.com";
	public static string Client_id = "59673991759-2rr5pslqilh0vdv35qts6ur89q3kh1gt.apps.googleusercontent.com";
	public static string Client_secret = "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";
	public static string CachedAccessToken = "ya29.a0AbVbY6OvA2tW1oUwSW-bS9DhZGUflYHOCSsklM0nWcqYdy3_yuSfHlMhb3HtFvaakstc-NlW1D9qBbZJ6g6eshQ6x3SFIzpIXqyQlmA8JZfQPUsU8G8lWXF4lX3LzN7hOSQ719UZTUKYfWybuqfRvzZm6TWuaCgYKAX4SARASFQFWKvPl0lWqKFf79ywAFVQ-qo574w0163";
	public static string CachedRefreshToken = "1//0gbHtJgRUJi6uCgYIARAAGBASNwF-L9Ir276lZUaDLG8k1Y8MGEd6dYY1s1WPHE8OEI2Yh_8kEuA7qbfZj9G9NL2-rS3r2GKLK3g";
	public static string ParentFolderImageHarianRespondenPagi = "1mMx3_IAlHblyWlAPseA4gn3fPlZPlrBg";
	public static string ParentFolderImageHarianRespondenMalam = "147yGkGVmdYkMvqplHPBN6huA8tJjNeMA";
	public static string ParentFolderImageFormGigiResponden = "1In_jbAcsnA1wl5feAz474eh_g24OdphE";
	public static string ParentFolderImageFormTindakanGigiResponden = "1r5Gd3Go48Y0FUVsn0U7jPJJFNarynyLs";
	public static string ParentFolderImageFormKontrolGigiResponden = "1lHoMhMKEKZdUGElPnlkOah1QGygkycev";
	public enum ImageUploadType
	{
		ImageHarian,
		ImageDebrisGigi,
		ImageTindakan
	}

	public static void SetTokenDrive()
	{
		GoogleDriveSettings drive = new GoogleDriveSettings();
		if (string.IsNullOrEmpty(drive.CachedAccessToken) || string.IsNullOrEmpty(drive.CachedRefreshToken))
		{
			drive.CachedAccessToken = CachedAccessToken;
			drive.CachedRefreshToken = CachedRefreshToken;
		}

	}


	/// <summary>
	/// fungsi untuk upload file foto ke drive
	/// </summary>
	/// <param name="_onDoneAction">method on success</param>
	/// <param name="_pathFile">location file image</param>
	/// <param name="_uploadtype">upload type nya</param>
	public static void UploadImageHarianResponden(Action<UnityGoogleDrive.Data.File> _onDoneAction, Action _onError, string _pathFile, string _fileName, bool _pagi)
	{
		var content = File.ReadAllBytes(_pathFile);
		if (content == null) return;
		var file = new UnityGoogleDrive.Data.File() { Name = _fileName, Content = content };

		string _useParent = (_pagi) ? ParentFolderImageHarianRespondenPagi : ParentFolderImageHarianRespondenMalam;
		file.Parents = new List<string> { _useParent };

		GoogleDriveFiles.CreateRequest request;
		request = GoogleDriveFiles.Create(file);
		request.Fields = new List<string> { "id", "name", "size", "createdTime" };
		request.Send().OnDone += (File) =>
		{
			if (request.IsError)
			{
				Debug.Log(request.Error);
				_onError.Invoke();
			}
			_onDoneAction(File);
		};
	}

	public static void UploadImageKontrolResponden(Action<UnityGoogleDrive.Data.File> _onDoneAction, int indx)
	{
		var content = File.ReadAllBytes(Debris.Instance.listFotoKontrol[indx]);
		Debug.Log(TestScript.Instance.listpathFoto.Count);
		if (content == null) return;
		string _fileName = $"{RespondenData.Instance.currentDataSelected.nama} - " + "Kontrol - " + indx;
		var file = new UnityGoogleDrive.Data.File() { Name = _fileName, Content = content };

		file.Parents = new List<string> { ParentFolderImageFormKontrolGigiResponden };

		GoogleDriveFiles.CreateRequest request;
		request = GoogleDriveFiles.Create(file);
		request.Fields = new List<string> { "id", "name", "size", "createdTime" };
		request.Send().OnDone += (File) =>
		{
			if (request.IsError)
			{
				Debug.Log(request.Error);
			}
			_onDoneAction(File);
		};
	}

	public static void UploadImageTindakanResponden(Action<UnityGoogleDrive.Data.File> _onDoneAction, int indx)
	{
		var content = File.ReadAllBytes(Jawaban.Instance.listFotoTindakan[indx]);
		Debug.Log(TestScript.Instance.listpathFoto.Count);
		if (content == null) return;
		string _fileName = $"{RespondenData.Instance.currentDataSelected.nama} - " + "Tindakan - " + indx;
		var file = new UnityGoogleDrive.Data.File() { Name = _fileName, Content = content };

		file.Parents = new List<string> { ParentFolderImageFormTindakanGigiResponden };

		GoogleDriveFiles.CreateRequest request;
		request = GoogleDriveFiles.Create(file);
		request.Fields = new List<string> { "id", "name", "size", "createdTime" };
		request.Send().OnDone += (File) =>
		{
			if (request.IsError)
			{
				Debug.Log(request.Error);
			}
			_onDoneAction(File);
		};
		RespondenData.Instance.RemoveFotoTindakan();
	}

	#endregion


	#region VIDEO INDEX
	static public int videoMateriIndx;
	#endregion

	public static string NamaDanSekolah()
	{
		return $"{RespondenData.Instance.currentDataSelected.nama}-{RespondenData.Instance.currentDataSelected.umur}";
	}
}