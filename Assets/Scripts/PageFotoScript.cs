using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class PageFotoScript : MonoBehaviour
{
    public enum StatusFotoGigi
    {
        PAGI,
        MALAM
    }
    public StatusFotoGigi statusTime;
    [SerializeField] private GameObject _objCameraTengah;
    [SerializeField] private GameObject _scrollViewImage;
    [SerializeField] private Transform _contentParentImage;
    [SerializeField] private Image _prefabImg;
    [SerializeField] private List<GameObject> _listObjImageCreated;
    public GameObject ImageShow;
    [SerializeField] private List<string> listUsed;

    public RespondenData.DataGambarGigi targetGigi;
    
    [Header("Pop Up")]
    public GameObject _popUpPanel;
    public TMP_Text _textPopUp;
    public Button DoneButton;
    
    #region MONOBEHAVIOUR FUNCTION

    public void OpenFoto(string _status)
    {
        if (_status.ToLower() == "pagi")
            statusTime = StatusFotoGigi.PAGI;
        else
            statusTime = StatusFotoGigi.MALAM;
    }

    private void OnEnable()
    {
        _objCameraTengah.SetActive(true);
        _scrollViewImage.SetActive(false);
        ClearDataImage();
    }

    #endregion

    void GetListImage()
    {
        //get the data image
        targetGigi = RespondenData.Instance.dataGambarGigi;

        listUsed = (statusTime == StatusFotoGigi.PAGI) ? targetGigi.listImageGigiPagi : targetGigi.listImageGigiMalam;

        /*if (listUsed.Count == 0) // datanya kosong
        {
            _objCameraTengah.SetActive(true);
            _scrollViewImage.SetActive(false);
            ClearDataImage();
        }
        else
        {
            _objCameraTengah.SetActive(false);
            _scrollViewImage.SetActive(true);
            ClearDataImage();

            //create the image
            foreach (var a in listUsed)
            {
                Texture2D tex = Helper.Base64ToTexture(a);
                CreateImage(tex);
            }

        }*/
    }

    void ClearDataImage()
    {
        foreach (var a in _listObjImageCreated)
            Destroy(a);
        _listObjImageCreated.Clear();
    }

    public void OpenGallery()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, -1);
                if (texture == null)
                {
                    print("Couldn't load texture from " + path);
                    return;
                }

                ImagePicked(texture, path);
            }
        });
    }

    private void ImagePicked(Texture2D _tex, string _path)
    {
        _scrollViewImage.SetActive(true);
        _objCameraTengah.SetActive(false);
        print($"Creating image from {_path}");
        if (Helper.CachedRefreshToken == "" || Helper.CachedAccessToken == "")
        {
            print("token empty");
            return;
        }
        PostImageHarianToDrive(_path, _tex);
    }
    
    public void PostImageHarianToDrive(string _path, Texture2D _tex)
    {
        //tampilkan loading
        DoneButton.gameObject.SetActive(false);
        PopUpMessage("Proses Upload..");

        string _fileName = "";
        string _time = (statusTime == StatusFotoGigi.PAGI) ? "Pagi" : "Malam";
        bool _pagi = (statusTime == StatusFotoGigi.PAGI) ? true : false;
        _fileName = $"{Helper.NamaDanSekolah()}-{_time}-{RespondenData.Instance.dataGambarGigi.listImageGigiPagi.Count + 1}";

        Helper.UploadImageHarianResponden((file) =>
        {
            PopUpMessage("Data berhasil diUpload");
            DoneButton.gameObject.SetActive(true);
            CreateImage(_tex, true);
        }, () => { Debug.Log("Error"); }, _path, _fileName, _pagi);
        //StartCoroutine(UploadImageHarianResponden(
        //    () =>
        //    {
        //        PemeliharaanSikatGigiManager.Instance.CloseLoading();
        //        PemeliharaanSikatGigiManager.Instance.SetTextMessage("Berhasil mengupload foto");
        //        CreateImage(_tex, true);
        //    },
        //    _path, _fileName
        //    ));
    }


    private void CreateImage(Texture2D _tex, bool save = false)
    {
        GameObject newImg = Instantiate(_prefabImg.gameObject, _contentParentImage);
        newImg.SetActive(true);

        Image _theImage = newImg.GetComponent<Image>();

        _theImage.preserveAspect = true;
        _theImage.SetNativeSize();

        //create sprite
        Sprite spriteFoto = Helper.TextureToSprite(_tex);
        _theImage.sprite = spriteFoto;

        //craete function Click the image
        newImg.GetComponent<Button>().onClick.AddListener(() =>
        {
            ImageShow.GetComponent<Image>().sprite = spriteFoto;
            ImageShow.GetComponent<Image>().preserveAspect = true;
            ImageShow.transform.parent.gameObject.SetActive(true);
        });

        //tampung ke list
        _listObjImageCreated.Add(newImg);

        //scroll to bottom       
        StartCoroutine(scrollToBottom());

        if (save)
        {
            string _imageSaved = Helper.TextureToBase64(_tex);
            /*RespondenData.Instance.dataGambarGigi.SaveGambar(_imageSaved, statusTime);*/
        }
    }

    IEnumerator scrollToBottom()
    {
        yield return null;
        _scrollViewImage.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
    }



    #region SEND IMAGE TO DRIVE
    //public IEnumerator UploadImageHarianResponden(Action _onDoneAction, string _pathFile, string _fileName)
    //{
    //    var drive = new GoogleDrive();
    //    drive.ClientID = Helper.Client_id;
    //    drive.ClientSecret = Helper.Client_secret;
    //    var authorization = drive.Authorize();
    //    yield return StartCoroutine(authorization);

    //    if (authorization.Current is Exception)
    //    {
    //        PemeliharaanSikatGigiManager.Instance.CloseLoading();
    //        PemeliharaanSikatGigiManager.Instance.SetTextMessage($"Gagal mengupload foto \n {authorization.Current.ToString()}");
    //        Debug.LogWarning(authorization.Current as Exception);
    //        yield break;
    //    }


    //    //drive.AccessToken = Helper.CachedAccessToken;
    //    //drive.RefreshToken = Helper.CachedRefreshToken;
    //    //drive.UserAccount = Helper.UserAccount;

    //    Dictionary<string, object>[] a = new Dictionary<string, object>[]
    // {
    //         new Dictionary<string, object>()
    //         {
    //             {"id", Helper.ParentFolderImageHarianResponden }
    //         }
    // };
    //    var file = new GoogleDrive.File(new Dictionary<string, object>
    //     {
    //       {"id",Helper.ParentFolderImageHarianResponden },
    //         { "mimeType","application/vnd.google-apps.folder"},
    //         {"parents", a }
    //     });

    //    var content = System.IO.File.ReadAllBytes(_pathFile);
    //    yield return StartCoroutine(drive.UploadFile(_fileName, "image/png", file, content));
    //    _onDoneAction();
    //}
    #endregion

    public void PopUpMessage(string message)
    {
        _popUpPanel.SetActive(true);
        _textPopUp.SetText(message);
    }
    
}