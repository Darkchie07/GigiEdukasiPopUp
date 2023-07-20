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
    public GameObject[] yes;
    public GameObject[] no;
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
    private bool isKontrol;

    [Header("Kontrol")]
    public List<Sprite> image = new List<Sprite>();
    public GameObject totalText;

    [Header("Tindakan")]
    public GameObject subField;

    [Header("Foto")]
    public List<string> liststringFoto = new List<string>();
    private List<string> listpathFoto = new List<string>();
    public GameObject PanelImage;
    public List<Sprite> sprData = new List<Sprite>();

    public List<string> ListpathFoto
    {
        get => listpathFoto;
        set => listpathFoto = value;
    }


    void Awake()
    {
        if (Instance == null)
            Instance = this;

        TagGObjects();
        // LoadData();
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

    private void Start()
    {
        if (isSikap)
        {
            if (Data.Instance.HasFile("Sikap"))
            {
                LoadJawaban();
            }
        }
        else if (isTindakan)
        {
            if (Data.Instance.HasFile("Tindakan"))
            {
                LoadJawaban();
            }
            if (Data.Instance.HasFile("FotoTindakan"))
            {
                LoadFoto();
            }
        }
        else if (isKontrol)
        {
            if (Data.Instance.HasFile("Kontrol"))
            {
                LoadJawaban();
            }
            if (Data.Instance.HasFile("FotoKontrol"))
            {
                LoadFoto();
            }
        }
        Debug.Log(Data.Instance.HasFile("Sikap"));
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
            isKontrol = true;
        }
    }

    public void GantiSoal()
    {
        NextButton.onClick.AddListener(() =>
        {
            if (isDone())
            {
                FillJawaban();
                SaveTest();
                if (isTindakan)
                {
                    SaveFoto();
                    Jawaban.Instance.UploadDataToDrive();
                }
                if (!isKontrol)
                    SceneManager.LoadScene(next);
                else
                {
                    SaveFoto();
                    Debris.Instance.UploadDebrisToDrive();
                }
            }
            else
                Debug.Log("Masih ada yg kurang " + Answered.ToString());
        });
        PrevButton.onClick.AddListener(() =>
        {
            if (isDone())
            {
                FillJawaban();
                SaveTest();
                if (isTindakan)
                {
                    SaveFoto();
                }
                SceneManager.LoadScene(prev);
            }
            else
                Debug.Log("Masih ada yg kurang " + Answered.ToString());
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
                listSkorDebris[i] = valtemp;
                temp += valtemp;
            }
            else
            {
                int valtemp = drop[i].GetComponent<TMP_Dropdown>().value;
                temp += valtemp;
            }
        }
        skor = temp;
        listSkorDebris[6] = skor;
        totalText.GetComponent<TMP_Text>().text = skor.ToString();
        SaveSkor();
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
        else if (isKontrol)
        {
            Debris.Instance.jawabanKontrol = listJawaban;
            Debris.Instance.skorKontrol = listSkorDebris;
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

    public void SaveTest()
    {
        // Data.Instance = new Data();
        // Data.Test d;
        // Data.Instance.dataTest = new Data.TestFile();
        // Data.Instance.dataTest.preTest = new Data.Pretest();
        // Data.Instance.dataTest.preTest.listPreTest = new List<Data.Test>();
        if (isSikap)
        {
            string json = JsonConvert.SerializeObject(listJawaban.ToArray());
            Data.Instance.SaveData("Sikap", json);
        }
        else if (isTindakan)
        {
            string json = JsonConvert.SerializeObject(listJawaban.ToArray());
            Data.Instance.SaveData("Tindakan", json);
        }
        else if (isKontrol)
        {
            string json = JsonConvert.SerializeObject(listJawaban.ToArray());
            Data.Instance.SaveData("Kontrol", json);
        }
        // Jaga jaga
        // d.Sikap = new string[listJawaban.Count]; // Instantiate the Sikap array
        // for (int i = 0; i < listJawaban.Count; i++)
        // {
        //     d.Sikap[i] = listJawaban[i];
        // }
        // Data.Instance.dataTest.preTest.listPreTest.Add(d);
        // Data.Instance.statusSikap = "1";
    }

    // public void LoadData()
    // {
    //     if (isPengetahuan && Data.Instance.statusPengetahuan == "1")
    //         for (int i = 0; i < Data.Instance.dataTest.preTest.listPreTest.Count; i++)
    //         {
    //             Pengetahuan.Instance.listJawaban[i] = Data.Instance.dataTest.preTest.listPreTest[i].Pengetahuan[i];
    //         }
    //     else if (isSikap && Data.Instance.statusSikap == "1")
    //         for (int i = 0; i < Data.Instance.dataTest.preTest.listPreTest.Count; i++)
    //         {
    //             listJawaban[i] = Data.Instance.dataTest.preTest.listPreTest[i].Sikap[i];
    //         }
    //     else if (isTindakan && Data.Instance.statusTindakan == "1")
    //         for (int i = 0; i < Data.Instance.dataTest.preTest.listPreTest.Count; i++)
    //         {
    //             listJawaban[i] = Data.Instance.dataTest.preTest.listPreTest[i].Tindakan[i];
    //         }
    //     else
    //         return;
    // }

    public void LoadJawaban()
    {
        string filePath = "";
        if (isSikap)
        {
            filePath = Application.persistentDataPath + "/saveTestSikap.json";
        }
        else if (isTindakan)
        {
            filePath = Application.persistentDataPath + "/saveTestTindakan.json";
        }
        else if (isKontrol)
        {
            filePath = Application.persistentDataPath + "/saveDebris.json";
        }
        string json = File.ReadAllText(filePath);
        List<string> jsonArray = JsonConvert.DeserializeObject<List<string>>(json);
        listJawaban = jsonArray;
        for (int i = 0; i < listJawaban.Count; i++)
        {
            if (listJawaban[i] == "0")
            {
                no[i].GetComponent<Toggle>().isOn = true;
            }
            else if (listJawaban[i] == "1")
            {
                yes[i].GetComponent<Toggle>().isOn = true;
            }
        }
    }

    public void SaveSkor()
    {
        if (isKontrol)
        {
            string json = JsonConvert.SerializeObject(listSkorDebris.ToArray());
            Data.Instance.SaveData("SkorKontrol", json);
        }
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
        print($"Creating image from {_path}");
        ListpathFoto[i] = _path;
        liststringFoto[i] = Helper.TextureToBase64(_tex);
        var a = i;
        CreateSprite(_tex, a);
    }
    private void CreateSprite(Texture2D _tex, int i)
    {
        //create sprite
        if (_tex != null)
        {
            foto[i].GetComponent<Button>().image.color = Color.red;
            Sprite spriteFoto = Helper.TextureToSprite(_tex);
            sprData[i] = spriteFoto;
        }
    }

    public void SaveFoto()
    {
        if (isTindakan)
        {
            Data.Instance.dataFoto.foto.StringFoto = liststringFoto.ToArray();
            Data.Instance.dataFoto.foto.PathFoto = ListpathFoto.ToArray();
            string json = JsonConvert.SerializeObject(Data.Instance.dataFoto.foto);
            Data.Instance.SaveData("FotoTindakan", json);
        }
        else if (isKontrol)
        {
            Data.Instance.dataFoto.foto.StringFoto = liststringFoto.ToArray();
            Data.Instance.dataFoto.foto.PathFoto = ListpathFoto.ToArray();
            string json = JsonConvert.SerializeObject(Data.Instance.dataFoto.foto);
            Data.Instance.SaveData("FotoKontrol", json);
        }
    }

    public void LoadFoto()
    {
        string filePath = "";
        if (isTindakan)
        {
            filePath = Application.persistentDataPath + "/saveFotoTindakan.json";
            string json = File.ReadAllText(filePath);
            Data.Instance.dataFoto.foto = JsonConvert.DeserializeObject<Data.FotoTindakan>(json);
        }
        else if (isKontrol)
        {
            filePath = Application.persistentDataPath + "/saveFotoKontrol.json";
            string json = File.ReadAllText(filePath);
            Data.Instance.dataFoto.foto = JsonConvert.DeserializeObject<Data.FotoTindakan>(json);
        }

        liststringFoto = Data.Instance.dataFoto.foto.StringFoto.ToList();
        ListpathFoto = Data.Instance.dataFoto.foto.PathFoto.ToList();

        for (int i = 0; i < liststringFoto.Count; i++)
        {
            //create sprite
            Texture2D tex = Helper.Base64ToTexture(liststringFoto[i]);
            ImagePicked(tex, i, ListpathFoto[i]);
        }


        // for (int i = 0; i < foto.Length; i++)
        // {
        //     liststringFoto[i] = .stringFoto;
        //     listpathFoto[i] = RespondenData.Instance.dataDebris.debris.listDebris[i].pathFoto;



        // }
        // btnSimpan.gameObject.SetActive(false);
    }
    #endregion
}
