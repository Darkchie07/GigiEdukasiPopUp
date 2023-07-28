using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Pengetahuan : MonoBehaviour
{
    public static string _typetest;
    public Image header;
    public Sprite preTest;
    public Sprite postTest;
    public GameObject jawaban;
    [System.Serializable]
    public class SoalPengetahuan
    {
        public string soal;
        public Sprite jawabanA;
        public Sprite jawabanB;
    }

    [Header("Soal")]
    public List<SoalPengetahuan> listsoal = new List<SoalPengetahuan>();

    [Header("Mount")]
    public TMP_Text txtSoal;
    public Image jawaban1;
    public Image jawaban2;
    public Button NextButton;
    public Button PrevButton;

    public int idxSoal;

    [Header("Jawaban")]
    public List<string> listJawaban = new List<string>();
    public List<string> listJawabanBenar = new List<string>();
    public int skor;

    [Header("Pop Up")] 
    public GameObject _popUpPanel;
    public TMP_Text _textPopUp;

    public static Pengetahuan Instance;


    void Start()
    {
        if (_typetest == "PreTest")
        {
            header.sprite = preTest;
        }else if (_typetest == "PostTest")
        {
            header.sprite = postTest;
        }
        if (Data.Instance.HasFile("Pengetahuan"))
        {
            LoadJawaban();
            HighlightJawaban();
        }
        if (Instance == null)
            Instance = this;
        TestScript.Instance.TagGObjects();
        // TestScript.Instance.LoadData();
        jawaban = GameObject.FindGameObjectWithTag("Jawaban");
        GenerateSoal(idxSoal);
        GantiSoal();
        AddJawaban();
        PrevButton.gameObject.SetActive(false);
    }


    private void GenerateSoal(int idx)
    {
        txtSoal.text = listsoal[idx].soal;
        jawaban1.sprite = listsoal[idx].jawabanA;
        jawaban2.sprite = listsoal[idx].jawabanB;
    }

    #region Tombol Next and Previous
    public void GantiSoal()
    {
        NextButton.onClick.AddListener(() =>
            {
                if (idxSoal >= listsoal.Count - 1)
                {
                    if (isDone())
                    {
                        CheckJawaban();
                        Jawaban.Instance.jawabanPengetahuan = listJawaban;
                        Jawaban.Instance.skorPengetahuan = skor.ToString();
                        string json = JsonConvert.SerializeObject(listJawaban.ToArray());
                        Data.Instance.SaveData("Pengetahuan", json);
                        Jawaban.Instance.skorTest[0] = skor.ToString();
                        SceneManager.LoadScene("Sikap");
                    }
                    else
                        PopUpMessage("Masih ada soal yang belum terjawab");

                }
                else if (0 < idxSoal && idxSoal < listsoal.Count)
                {
                    idxSoal += 1;
                    GenerateSoal(idxSoal);
                    HighlightJawaban();
                }
                else if (idxSoal == 0)
                {
                    idxSoal += 1;
                    GenerateSoal(idxSoal);
                    PrevButton.gameObject.SetActive(true);
                    HighlightJawaban();
                }
            });
        PrevButton.onClick.AddListener(() =>
        {
            if (idxSoal >= listsoal.Count)
            {
                idxSoal -= 1;
                GenerateSoal(idxSoal);
                HighlightJawaban();
            }
            else if (2 <= idxSoal && idxSoal < listsoal.Count)
            {
                idxSoal -= 1;
                GenerateSoal(idxSoal);
                HighlightJawaban();
            }
            else if (idxSoal == 1)
            {
                idxSoal -= 1;
                GenerateSoal(idxSoal);
                PrevButton.gameObject.SetActive(false);
                HighlightJawaban();
            }
        });

    }
    #endregion

    #region Highlight Jawaban Ketika Ganti Soal
    public void HighlightJawaban()
    {
        if (listJawaban[idxSoal] != "-")
        {
            if (listJawaban[idxSoal] == "A")
            {
                var colorsHighlight1 = jawaban1.GetComponent<Button>().colors;
                var colorsHighlight2 = jawaban2.GetComponent<Button>().colors;
                colorsHighlight2.normalColor = Color.white;
                colorsHighlight1.normalColor = Color.green;
                jawaban1.GetComponent<Button>().colors = colorsHighlight1;
                jawaban2.GetComponent<Button>().colors = colorsHighlight2;
            }
            else if (listJawaban[idxSoal] == "B")
            {
                var colorsHighlight1 = jawaban1.GetComponent<Button>().colors;
                var colorsHighlight2 = jawaban2.GetComponent<Button>().colors;
                colorsHighlight2.normalColor = Color.white;
                colorsHighlight1.normalColor = Color.green;
                jawaban1.GetComponent<Button>().colors = colorsHighlight2;
                jawaban2.GetComponent<Button>().colors = colorsHighlight1;
            }
        }
        else
        {
            var colorsHighlight = jawaban1.GetComponent<Button>().colors;
            colorsHighlight.normalColor = Color.white;
            jawaban1.GetComponent<Button>().colors = colorsHighlight;
            jawaban2.GetComponent<Button>().colors = colorsHighlight;
        }
    }
    #endregion

    #region Ketika menjawab atau mengubah jawaban
    public void AddJawaban()
    {
        jawaban1.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (listJawaban[idxSoal] == "B")
            {
                var colorsHighlight1 = jawaban1.GetComponent<Button>().colors;
                var colorsHighlight2 = jawaban2.GetComponent<Button>().colors;
                colorsHighlight2.normalColor = Color.white;
                colorsHighlight1.selectedColor = Color.green;
                jawaban1.GetComponent<Button>().colors = colorsHighlight1;
                jawaban2.GetComponent<Button>().colors = colorsHighlight2;
            }
            listJawaban[idxSoal] = "A";
        });
        jawaban2.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (listJawaban[idxSoal] == "A")
            {
                var colorsHighlight1 = jawaban1.GetComponent<Button>().colors;
                var colorsHighlight2 = jawaban2.GetComponent<Button>().colors;
                colorsHighlight1.normalColor = Color.white;
                colorsHighlight2.selectedColor = Color.green;
                jawaban1.GetComponent<Button>().colors = colorsHighlight2;
                jawaban2.GetComponent<Button>().colors = colorsHighlight1;
            }
            listJawaban[idxSoal] = "B";
        });
    }
    #endregion

    public void LoadJawaban()
    {
        string filePath = Application.persistentDataPath + "/saveTestPengetahuan.json";
        string json = File.ReadAllText(filePath);
        List<string> jsonArray = JsonConvert.DeserializeObject<List<string>>(json);
        listJawaban = jsonArray;
    }

    #region Check Jawaban setelah semua soal
    public void CheckJawaban()
    {
        for (int i = 0; i < listJawabanBenar.Count; i++)
        {
            if (listJawabanBenar[i] == listJawaban[i])
            {
                skor += 1;
            }
        }
        Debug.Log(skor);
    }

    private bool isDone()
    {
        if (listJawaban.Contains("-"))
        {
            return false;
        }
        return true;
    }
    #endregion

    public void PopUpMessage(string message)
    {
        _popUpPanel.SetActive(true);
        _textPopUp.SetText(message);
    }
}
