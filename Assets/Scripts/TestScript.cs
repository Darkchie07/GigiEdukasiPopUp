using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.Networking;

public class TestScript : MonoBehaviour
{
    public static TestScript Instance;
    public GameObject jawaban;
    public GameObject[] index;

    GameObject field;
    GameObject content;

    [System.Serializable]
    public class SoalSikap
    {
        public string soal;
        // public Button yes;
        // public Button no;
    }

    [Header("Soal")]
    public List<SoalSikap> listSoal = new List<SoalSikap>();
    GameObject[] yes;
    GameObject[] no;
    GameObject[] drop;
    public GameObject[] foto;

    [Header("Mount")]
    public Button NextButton;
    public Button PrevButton;
    public List<GameObject> listtxtSoal;
    public List<string> listJawaban = new List<string>();
    private List<int> listJawabanYa = new List<int>();
    private List<int> listJawabanNo = new List<int>();
    public List<int> listSkorDebris = new List<int>();
    public int Answered;
    public int skor;
    public string next;
    public string prev;
    private bool isPengetahuan;
    private bool isSikap;
    private bool isTindakan;

    [Header("Kontrol")]
    public List<Sprite> image = new List<Sprite>();
    public GameObject totalText;
    public int nilai;
    public List<int> listnilai = new List<int>();

    [Header("Tindakan")]
    public GameObject subField;

    [Header("Foto")]
    public List<string> liststringFoto = new List<string>();
    public List<string> listpathFoto = new List<string>();
    public GameObject PanelImage;
    public List<Sprite> sprData = new List<Sprite>();

    void Start()
    {
        if (Instance == null)
            Instance = this;

        TagGObjects();
        LoadData();
        if (!isPengetahuan)
        {
            field = content.transform.GetChild(0).gameObject;
            Transform parent = content.transform;

            DuplicateField(parent);
            index = GameObject.FindGameObjectsWithTag("Field");
            yes = GameObject.FindGameObjectsWithTag("Ya");
            no = GameObject.FindGameObjectsWithTag("Tidak");
            drop = GameObject.FindGameObjectsWithTag("Dropdown");
            foto = GameObject.FindGameObjectsWithTag("Foto");

            GantiSoal();
            SetSoal();
        }

        // FillJawaban();
    }

    public void TagGObjects()
    {
        jawaban = GameObject.FindGameObjectWithTag("Jawaban");
        if (GameObject.FindGameObjectWithTag("Pengetahuan") != null)
        {
            content = GameObject.FindGameObjectWithTag("Pengetahuan").gameObject;
            isPengetahuan = true;
        }
        else if (GameObject.FindGameObjectWithTag("Sikap") != null)
        {
            content = GameObject.FindGameObjectWithTag("Sikap").gameObject;
            isSikap = true;
        }
        else if (GameObject.FindGameObjectWithTag("Tindakan") != null)
        {
            content = GameObject.FindGameObjectWithTag("Tindakan").gameObject;
            isTindakan = true;
        }
        else if (GameObject.FindGameObjectWithTag("Kontrol") != null)
        {
            content = GameObject.FindGameObjectWithTag("Kontrol").gameObject;
            isTindakan = true;
        }
    }

    public void GantiSoal()
    {
        NextButton.onClick.AddListener(() =>
        {
            if (isDone())
            {
                FillJawaban();
                SaveData();
                if (isTindakan)
                {
                    Jawaban.Instance.UploadDataToDrive();
                }
                else
                {
                    SceneManager.LoadScene(next);
                }
            }
            else
                Debug.Log("Masih ada yg kurang" + Answered.ToString());
        });
        PrevButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(prev);
        });
        if (content.CompareTag("Tindakan"))
        {
            for (int i = 0; i < foto.Length; i++)
            {
                var a = i;
                foto[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    takePic(a);
                });
            }
        }

        if (content.CompareTag("Kontrol"))
        {
            for (int i = 0; i < drop.Length; i++)
            {
                drop[i].GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { changeSkor(); });
            }
        }
    }

    private void takePic(int i)
    {
        if (!string.IsNullOrEmpty(liststringFoto[i]))
        {
            PanelImage.SetActive(true);
            PanelImage.transform.GetChild(1).GetComponent<Image>().sprite = sprData[i];
            // PemeliharaanSikatGigiManager.Instance.OrientationToAuto();
        }
        else
            OpenGallery(i);
    }

    public void changeSkor()
    {
        int temp = 0;
        for (int i = 0; i < drop.Length; i++)
        {
            if (drop[i].GetComponent<TMP_Dropdown>().value != 0)
            {
                int valtemp = drop[i].GetComponent<TMP_Dropdown>().value - 1;
                temp += valtemp;
            }
            else
            {
                int valtemp = drop[i].GetComponent<TMP_Dropdown>().value;
                temp += valtemp;
            }
        }
        skor = temp;
        totalText.GetComponent<TMP_Text>().text = skor.ToString();
    }

    #region Set Soal Awal
    public void SetSoal()
    {
        for (int i = 0; i < index.Length; i++)
        {
            int no = i + 1;
            index[i].transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = no.ToString();
            index[i].transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = listSoal[i].soal;
        }

        if (content.CompareTag("Kontrol"))
        {
            for (int i = 0; i < index.Length; i++)
                index[i].transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Image>().sprite = image[i];
        }
    }
    #endregion

    #region SetJawaban

    public void FillJawaban()
    {
        for (int i = 0; i < listJawabanNo.Count; i++)
        {
            listJawaban[listJawabanNo[i]] = "0";
        }
        for (int i = 0; i < listJawabanYa.Count; i++)
        {
            listJawaban[listJawabanYa[i]] = "1";
        }

        if (isSikap)
        {
            Jawaban.Instance.jawabanSikap = listJawaban;
        }
        else if (isTindakan)
        {
            Jawaban.Instance.jawabanTindakan = listJawaban;
        }
    }
    #endregion

    public void DuplicateField(Transform parent)
    {
        for (int i = 1; i < listtxtSoal.Count; i++)
        {
            if (content.CompareTag("Tindakan"))
            {
                if (i == 13)
                {
                    var subBaris = Instantiate(subField, parent);
                    subBaris.transform.GetChild(0).GetComponent<TMP_Text>().text = "BAGIAN MENGHADAP BIBIR";
                }
                else if (i == 15)
                {
                    var subBaris = Instantiate(subField, parent);
                    subBaris.transform.GetChild(0).GetComponent<TMP_Text>().text = "BAGIAN MENGHADAP PIPI";
                }
                else if (i == 19)
                {
                    var subBaris = Instantiate(subField, parent);
                    subBaris.transform.GetChild(0).GetComponent<TMP_Text>().text = "BAGIAN MENGHADAP LIDAH";
                }
                else if (i == 21)
                {
                    var subBaris = Instantiate(subField, parent);
                    subBaris.transform.GetChild(0).GetComponent<TMP_Text>().text = "BAGIAN MENGHADAP LANGIT-LANGIT";
                }
                else if (i == 23)
                {
                    var subBaris = Instantiate(subField, parent);
                    subBaris.transform.GetChild(0).GetComponent<TMP_Text>().text = "BAGIAN KUNYAH";
                }
                else if (i == 27)
                {
                    var subBaris = Instantiate(subField, parent);
                    subBaris.transform.GetChild(0).GetComponent<TMP_Text>().text = "BAGIAN LIDAH";
                }
            }
            var baris = Instantiate(field, parent);
            listtxtSoal[0] = field;
            listtxtSoal[i] = baris;
        }
    }

    private bool isDone()
    {
        for (int i = 0; i < yes.Length; i++)
        {
            if (yes[i].GetComponent<Toggle>().isOn)
            {
                Answered += 1;
                listJawabanYa.Add(i);
            }
        }
        for (int i = 0; i < no.Length; i++)
        {
            if (no[i].GetComponent<Toggle>().isOn)
            {
                Answered += 1;
                listJawabanNo.Add(i);
            }
        }
        if (Answered == listSoal.Count)
        {
            return true;
        }

        return false;
    }

    public void SaveData()
    {
        Data.Instance = new Data();
        Data.Instance.dataTest = new Data.TestFile();
        Data.Instance.dataTest.preTest = new Data.Pretest();
        Data.Instance.dataTest.preTest.listPreTest = new List<Data.Test>();
        if (isPengetahuan)
        {
            Data.Test d = new Data.Test();
            d.Pengetahuan = new string[Pengetahuan.Instance.listJawaban.Count]; // Instantiate the Sikap array
            for (int i = 0; i < Pengetahuan.Instance.listJawaban.Count; i++)
            {
                d.Pengetahuan[i] = Pengetahuan.Instance.listJawaban[i];
            }
            Data.Instance.dataTest.preTest.listPreTest.Add(d);
            Data.Instance.statusPengetahuan = "1";
        }
        if (isSikap)
        {
            Data.Test d = new Data.Test();
            d.Sikap = new string[listJawaban.Count]; // Instantiate the Sikap array
            for (int i = 0; i < listJawaban.Count; i++)
            {
                d.Sikap[i] = listJawaban[i];
            }
            Data.Instance.dataTest.preTest.listPreTest.Add(d);
            Data.Instance.statusSikap = "1";
        }
        else if (isTindakan)
        {
            Data.Test d = new Data.Test();
            d.Tindakan = new string[listJawaban.Count]; // Instantiate the Sikap array
            for (int i = 0; i < listJawaban.Count; i++)
            {
                d.Tindakan[i] = listJawaban[i];
            }
            Data.Instance.dataTest.preTest.listPreTest.Add(d);
            Data.Instance.statusSikap = "1";
        }
        string json = JsonConvert.SerializeObject(Data.Instance.dataTest);
        Debug.Log(Application.persistentDataPath);
        Data.Instance.SaveTest(json);
    }

    public void LoadData()
    {
        if (isPengetahuan && Data.Instance.statusPengetahuan == "1")
            for (int i = 0; i < Data.Instance.dataTest.preTest.listPreTest.Count; i++)
            {
                Pengetahuan.Instance.listJawaban[i] = Data.Instance.dataTest.preTest.listPreTest[i].Pengetahuan[i];
            }
        else if (isSikap && Data.Instance.statusSikap == "1")
            for (int i = 0; i < Data.Instance.dataTest.preTest.listPreTest.Count; i++)
            {
                listJawaban[i] = Data.Instance.dataTest.preTest.listPreTest[i].Sikap[i];
            }
        else if (isTindakan && Data.Instance.statusTindakan == "1")
            for (int i = 0; i < Data.Instance.dataTest.preTest.listPreTest.Count; i++)
            {
                listJawaban[i] = Data.Instance.dataTest.preTest.listPreTest[i].Tindakan[i];
            }
        else
            return;
    }

    #region Foto
    private void OpenGallery(int i)
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

                    ImagePicked(texture, i, path);
                }
            });
    }

    public void ImagePicked(Texture2D _tex, int i, string _path = "")
    {
        string stringFoto;
        string pathFoto;
        print($"Creating image from {_path}");
        stringFoto = _path;
        listpathFoto[i] = stringFoto;
        pathFoto = Helper.TextureToBase64(_tex);
        liststringFoto[i] = pathFoto;
        var a = i;
        CreateSprite(_tex, a);
    }
    private void CreateSprite(Texture2D _tex, int i)
    {
        //create sprite
        // txtButtonFoto.text = "Lihat Foto";
        Sprite spriteFoto = Helper.TextureToSprite(_tex);
        sprData[i] = spriteFoto;
    }

    // public void LoadData()
    // {
    //     for (int i = 0; i < foto.Length; i++)
    //     {
    //         liststringFoto[i] = .stringFoto;
    //         listpathFoto[i] = RespondenData.Instance.dataDebris.debris.listDebris[i].pathFoto;

    //         //create sprite
    //         Texture2D tex = Helper.Base64ToTexture(listDataGigiDebris[i].stringFoto);
    //         listDataGigiDebris[i].ImagePicked(tex);
    //     }
    //     btnSimpan.gameObject.SetActive(false);


    // }
    #endregion
}
