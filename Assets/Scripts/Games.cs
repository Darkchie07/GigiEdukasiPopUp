using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Games : MonoBehaviour
{
    [System.Serializable]
    public class TBGames
    {
        public string soal;
        public GameObject gambarSoal;
        public List<string> simpanListJawaban = new List<string>();
    }

    [Header("Soal")]
    public List<TBGames> listGambarSoal = new List<TBGames>();

    [Header("Mount")]
    public TMP_Text txtSoal;
    public Button NextButton;
    public Button PrevButton;
    public int idxSoal;

    [Header("Jawaban")]
    public List<string> listJawaban = new List<string>();
    public List<string> listJawabanBenar = new List<string>();
    bool isCorrect = false;
    bool isAnswered = false;

    [Header("Button")]
    public List<Button> buttonList;
    private string buttonValue;
    bool isClicked;

    [Header("Save Load")]
    private int SaveListCount;

    void Start()
    {
        LoadList();
        GantiSoal();
        AddJawaban();
        if (PlayerPrefs.GetInt("IdxLevel") == 0)
            PrevButton.gameObject.SetActive(false);

        if (PlayerPrefs.HasKey("IdxLevel"))
            HighlightJawaban();
    }

    public void GenerateSoal(int idx)
    {
        isCorrect = false;
        txtSoal.text = listGambarSoal[idx].soal;
        listGambarSoal[idx].gambarSoal.SetActive(true);
    }

    #region Tombol Next and Previous
    public void GantiSoal()
    {
        NextButton.onClick.AddListener(() =>
        {
            SaveList();

            if (listJawaban[idxSoal] == listJawabanBenar[idxSoal])
            {
                isCorrect = true;
            }

            if (isCorrect == true)
            {
                listGambarSoal[idxSoal].gambarSoal.SetActive(false);

                if (idxSoal >= listGambarSoal.Count - 1)
                {
                    // if (isDone())
                    // {
                    //     CheckJawaban();
                    SceneManager.LoadScene("PemeliharaanGigi");
                    // }
                }
                else if (0 < idxSoal && idxSoal < listGambarSoal.Count)
                {
                    idxSoal += 1;
                    GenerateSoal(idxSoal);
                    ResetHighlight();
                }
                else if (idxSoal == 0)
                {
                    idxSoal += 1;
                    GenerateSoal(idxSoal);
                    ResetHighlight();
                    PrevButton.gameObject.SetActive(true);
                }
            }
            PlayerPrefs.SetInt("IdxLevel", idxSoal);
        });

        PrevButton.onClick.AddListener(() =>
        {
            listGambarSoal[idxSoal].gambarSoal.SetActive(false);

            if (idxSoal >= listGambarSoal.Count)
            {
                idxSoal -= 1;
                GenerateSoal(idxSoal);
                HighlightJawaban();
            }
            else if (2 <= idxSoal && idxSoal < listGambarSoal.Count)
            {
                idxSoal -= 1;
                GenerateSoal(idxSoal);
                HighlightJawaban();
            }
            else if (idxSoal == 1)
            {
                idxSoal -= 1;
                GenerateSoal(idxSoal);
                HighlightJawaban();
                PrevButton.gameObject.SetActive(false);
            }
            PlayerPrefs.SetInt("IdxLevel", idxSoal);
        });
    }
    #endregion

    #region Highlight Jawaban Ketika Ganti Soal
    public void HighlightJawaban()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            var buttonClick = buttonList[i];
            var colorsHighlight = buttonClick.GetComponent<Button>().colors;

            if (listJawaban[idxSoal] == buttonList[i].gameObject.tag)
            {
                colorsHighlight.normalColor = Color.green;
                buttonClick.GetComponent<Button>().colors = colorsHighlight;
            }

            if (buttonList[i].gameObject.tag != listJawaban[idxSoal])
            {
                colorsHighlight.normalColor = Color.white;
                buttonClick.GetComponent<Button>().colors = colorsHighlight;
            }
        }
    }
    #endregion

    #region Reset Highlight Jawaban Ketika Ganti Soal
    public void ResetHighlight()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            var buttonClick = buttonList[i];
            var colorsHighlight = buttonClick.GetComponent<Button>().colors;

            if (listJawaban[idxSoal] == buttonList[i].gameObject.tag)
            {
                colorsHighlight.normalColor = Color.green;
                buttonClick.GetComponent<Button>().colors = colorsHighlight;
            }

            if (buttonList[i].gameObject.tag != listJawaban[idxSoal])
            {
                colorsHighlight.normalColor = Color.white;
                buttonClick.GetComponent<Button>().colors = colorsHighlight;
            }
        }
    }
    #endregion

    #region Ketika menjawab atau mengubah jawaban
    public void AddJawaban()
    {
        for (int i = 0; i < buttonList.Count; i++)
        // foreach (var buttonClick in buttonList)
        {
            Button buttonClick = buttonList[i];

            buttonClick.onClick.AddListener(() =>
            {
                isClicked = true;

                if (isClicked)
                {
                    if (buttonClick.name == "MencungkilAtas")
                    {
                        buttonValue = "A";
                        ButtonHighLight(buttonClick);
                    }
                    else if (buttonClick.name == "Memutar")
                    {
                        buttonValue = "B";
                        ButtonHighLight(buttonClick);
                    }
                    else if (buttonClick.name == "MajuMundur")
                    {
                        buttonValue = "C";
                        ButtonHighLight(buttonClick);
                    }
                    else if (buttonClick.name == "KeLuar")
                    {
                        buttonValue = "D";
                        ButtonHighLight(buttonClick);
                    }
                    else if (buttonClick.name == "AtasBawah")
                    {
                        buttonValue = "E";
                        ButtonHighLight(buttonClick);
                    }
                    else if (buttonClick.name == "MencungkilBawah")
                    {
                        buttonValue = "F";
                        ButtonHighLight(buttonClick);
                    }
                }
                CheckJawaban(buttonValue);
                Debug.Log("Button value " + buttonValue);
            });
        }
    }
    #endregion

    #region Check Jawaban setelah semua soal
    public void CheckJawaban(string buttonValue)
    {
        listJawaban[idxSoal] = buttonValue;

        if (buttonValue == listJawabanBenar[idxSoal])
        {
            isCorrect = true;
            // listJawaban[idxSoal] = buttonValue;
        }
        Debug.Log("isCorrect " + isCorrect);
    }

    // private bool isDone()
    // {
    //     if (jawabanPlayer.Contains("-"))
    //     {
    //         return false;
    //     }
    //     return true;
    // }
    #endregion

    private void ButtonHighLight(Button buttonClick)
    {
        var colorsHighlight = buttonClick.GetComponent<Button>().colors;

        colorsHighlight.selectedColor = Color.green;

        if (colorsHighlight.Equals(Color.green))
        {
            colorsHighlight.selectedColor = Color.white;
            buttonClick.GetComponent<Button>().colors = colorsHighlight;
        }
        else
        {
            colorsHighlight.selectedColor = Color.green;
            buttonClick.GetComponent<Button>().colors = colorsHighlight;
        }
    }

    public void SaveList()
    {
        for (int i = 0; i < listJawaban.Count; i++)
        {
            PlayerPrefs.SetString("Jawaban" + i, listJawaban[i]);
        }
        PlayerPrefs.SetInt("Count", listJawaban.Count);
    }

    public void LoadList()
    {
        SaveListCount = PlayerPrefs.GetInt("Count");

        if (PlayerPrefs.HasKey("IdxLevel"))
        {
            idxSoal = PlayerPrefs.GetInt("IdxLevel");
            GenerateSoal(idxSoal);
        }
        else
        {
            idxSoal = 0;
            GenerateSoal(idxSoal);
        }

        for (int i = 0; i < SaveListCount; i++)
        {
            string jawaban = PlayerPrefs.GetString("Jawaban" + i);
            listJawaban[i] = jawaban;
        }
    }
}