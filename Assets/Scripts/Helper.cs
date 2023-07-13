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
        ChangeScene("Scenes/Home");
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
    private static string UrlFormRespondenDataDebris = "https://docs.google.com/forms/u/4/d/e/1FAIpQLSdBZn60DLRpN5TZ25y5k8KllpqyAVmdcOUJTL7l6jbURBlDcA/formResponse";
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

    public static IEnumerator CoroutineUploadFormGigiResponden(string _grahamKananAtas, string _depanAtas, string _grahamKiriAtas, string _grahamKiriBawah, string _depanBawah, string _grahamKananBawah, Action _success, Action<UnityWebRequest> _error)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.477203861", RespondenData.Instance.currentDataSelected.nama);
        form.AddField("entry.878239228", RespondenData.Instance.currentDataSelected.umur);
        form.AddField("entry.275371160", (RespondenData.Instance.currentDataSelected.jenisKelamin == "0") ? "Laki-Laki" : "Perempuan");
        form.AddField("entry.1655155096", _grahamKananAtas);
        form.AddField("entry.1777940349", _depanAtas);
        form.AddField("entry.1459528608", _grahamKiriAtas);
        form.AddField("entry.812763402", _grahamKiriBawah);
        form.AddField("entry.924637615", _depanBawah);
        form.AddField("entry.1743842660", _grahamKananBawah);

        UnityWebRequest www = UnityWebRequest.Post(UrlFormRespondenDataDebris, form);


        yield return www.SendWebRequest();


        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            _error(www);
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
    public static string CachedAccessToken = "ya29.a0AbVbY6NfRLNz2xvCzqDsOBekBP-3Ca969m0mKWkTD9g2u8y20Kaer0kU7GApt8QTksiOzPM0U5JQNDjupURZTfBx8W4DaoFOsbW5L1XXUC4L_9zcswT5Tpa-nBQjdsbD46lobyqQpzYrhYW3Nx6WO5AwG-JKaCgYKAYgSARASFQFWKvPl7pDAR2LivkLp_FTYv7rx-A0163";
    public static string CachedRefreshToken = "1//0grYz56SccvFlCgYIARAAGBASNwF-L9IrSqcLIxViKSmqpAbpJk4OY43SvH6Pa7yYoSIYvGjudhBoIFXKMGJ_TGMn88ApeY0qd54";
    public static string ParentFolderImageHarianRespondenPagi = "1mMx3_IAlHblyWlAPseA4gn3fPlZPlrBg";
    public static string ParentFolderImageHarianRespondenMalam = "147yGkGVmdYkMvqplHPBN6huA8tJjNeMA";
    public static string ParentFolderImageFormGigiResponden = "1In_jbAcsnA1wl5feAz474eh_g24OdphE";
    public enum ImageUploadType
    {
        ImageHarian,
        ImageJenisGigi
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

    public static void UploadFotoDebris(Action<UnityGoogleDrive.Data.File> _onDoneAction, int indx)
    {
        var content = File.ReadAllBytes(RespondenData.Instance.dataDebris.debris.listDebris[indx].pathFoto);
        if (content == null) return;

        string _fileName = $"{NamaDanSekolah()}-{RespondenData.Instance.dataDebris.debris.listDebris[indx].namaGigi}";

        var file = new UnityGoogleDrive.Data.File() { Name = _fileName, Content = content };

        file.Parents = new List<string> { ParentFolderImageFormGigiResponden };

        GoogleDriveFiles.CreateRequest request;
        request = GoogleDriveFiles.Create(file);
        request.Fields = new List<string> { "id", "name", "size", "createdTime" };
        request.Send().OnDone += (File) =>
        {
            indx++;
            if (indx > 5)
                _onDoneAction.Invoke(File);
            else
                UploadFotoDebris(_onDoneAction, indx);
        };
    }

    #endregion

    public static string NamaDanSekolah()
    {
        return $"{RespondenData.Instance.currentDataSelected.nama}";
    }


    #region VIDEO INDEX
    static public int videoMateriIndx;
    #endregion
}