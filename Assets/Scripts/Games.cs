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
    public List<GameObject> listGambarSikat = new List<GameObject>();

    [Header("Mount")]
    public TMP_Text txtSoal;
    public Button NextButton;
    public Button PrevButton;
    public int idxSoal;

    [Header("Jawaban")]
    public List<string> listJawaban = new List<string>();
    public List<string> listJawabanBenar = new List<string>();
    bool isCorrect = false;
    // bool isAnswered = false;

    [Header("Button")]
    public List<Button> buttonList;
    private Animator animSikat;
    private string buttonValue;
    bool isClicked;

    [Header("Save Load")]
    private int SaveListCount;

    [Header("Particle")]
    ParticleSystem bubble;

    [Header("AudioSFX")]
    public GameObject audioSFX;
    AudioSource audioSource;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    void Start()
    {
        LoadList();
        GantiSoal();
        AddJawaban();
        if (PlayerPrefs.GetInt("IdxLevel") == 0)
            PrevButton.gameObject.SetActive(false);

        if (PlayerPrefs.HasKey("IdxLevel"))
            HighlightJawaban();

        audioSource = audioSFX.GetComponent<AudioSource>();
    }

    public void GenerateSoal(int idx)
    {
        isCorrect = false;
        txtSoal.text = listGambarSoal[idx].soal;
        bubble = listGambarSikat[idx].transform.GetChild(0).GetComponent<ParticleSystem>();
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
                if (idxSoal >= listGambarSoal.Count - 1)
                {
                    // if (isDone())
                    // {
                    //     CheckJawaban();
                    SceneManager.LoadScene("Home");
                    // }
                }
                else if (0 < idxSoal && idxSoal < listGambarSoal.Count)
                {
                    listGambarSoal[idxSoal].gambarSoal.SetActive(false);
                    idxSoal += 1;
                    GenerateSoal(idxSoal);
                    ResetHighlight();
                }
                else if (idxSoal == 0)
                {
                    listGambarSoal[idxSoal].gambarSoal.SetActive(false);
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

    #region Highlight Jawaban Ketika Ganti Soal dan Load Save
    public void HighlightJawaban()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            var buttonClick = buttonList[i];
            var colorsHighlight = buttonClick.GetComponent<Button>().colors;

            if (listJawaban[idxSoal] == buttonList[i].gameObject.tag && listJawaban[idxSoal] == listJawabanBenar[idxSoal])
            {
                colorsHighlight.normalColor = Color.green;
                buttonClick.GetComponent<Button>().colors = colorsHighlight;
            }
            else
            {
                colorsHighlight.normalColor = Color.white;
                buttonClick.GetComponent<Button>().colors = colorsHighlight;
            }

            // if (buttonList[i].gameObject.tag != listJawaban[idxSoal])
            // {
            // }
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

            if (listJawaban[idxSoal] == buttonList[i].gameObject.tag && listJawaban[idxSoal] == listJawabanBenar[idxSoal])
            {
                colorsHighlight.normalColor = Color.green;
                buttonClick.GetComponent<Button>().colors = colorsHighlight;
            }
            else
            {
                colorsHighlight.normalColor = Color.white;
                buttonClick.GetComponent<Button>().colors = colorsHighlight;
            }

            // if (buttonList[i].gameObject.tag != listJawaban[idxSoal])
            // {
            //     colorsHighlight.normalColor = Color.white;
            //     buttonClick.GetComponent<Button>().colors = colorsHighlight;
            // }
        }
    }
    #endregion

    private void Update()
    {
        animSikat = listGambarSikat[idxSoal].GetComponent<Animator>();
    }

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
                    bubble.Play();

                    if (buttonClick.name == "MencungkilAtas")
                    {
                        buttonValue = "A";
                        animSikat.Play("MencungkilAtas");
                        ButtonHighLight(buttonClick, buttonValue);
                    }
                    else if (buttonClick.name == "Memutar")
                    {
                        buttonValue = "B";
                        animSikat.Play("Memutar");
                        ButtonHighLight(buttonClick, buttonValue);
                    }
                    else if (buttonClick.name == "MajuMundur")
                    {
                        buttonValue = "C";
                        animSikat.Play("MajuMundur");
                        ButtonHighLight(buttonClick, buttonValue);
                    }
                    else if (buttonClick.name == "KeLuar")
                    {
                        buttonValue = "D";
                        animSikat.Play("KeLuar");
                        ButtonHighLight(buttonClick, buttonValue);
                    }
                    else if (buttonClick.name == "AtasBawah")
                    {
                        buttonValue = "E";
                        animSikat.Play("AtasBawah");
                        ButtonHighLight(buttonClick, buttonValue);
                    }
                    else if (buttonClick.name == "MencungkilBawah")
                    {
                        buttonValue = "F";
                        animSikat.Play("MencungkilBawah");
                        ButtonHighLight(buttonClick, buttonValue);
                    }
                    // CheckJawaban(buttonValue);
                }
            });
        }
    }
    #endregion

    public void GoWhite()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            var buttonClick = buttonList[i];
            var colorsHighlight = buttonClick.GetComponent<Button>().colors;

            if (buttonList[i].colors.normalColor.Equals(Color.green))
            {
                colorsHighlight.normalColor = Color.white;
                buttonList[i].GetComponent<Button>().colors = colorsHighlight;
            }
        }
    }

    #region Check Jawaban setelah semua soal
    public void CheckJawaban(string buttonValue)
    {
        listJawaban[idxSoal] = buttonValue;

        if (listJawaban[idxSoal] == listJawabanBenar[idxSoal])
        {
            audioSource.clip = correctSFX;
            audioSource.Play();
            isCorrect = true;
            // listJawaban[idxSoal] = buttonValue;
        }
        else
        {
            audioSource.clip = wrongSFX;
            audioSource.Play();
            isCorrect = false;
        }
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

    private void ButtonHighLight(Button buttonClick, string buttonValue)
    {
        CheckJawaban(buttonValue);
        GoWhite();

        var colorsHighlight = buttonClick.GetComponent<Button>().colors;

        // if (isCorrect == true)
        //     colorsHighlight.selectedColor = Color.green;
        // else
        //     colorsHighlight.selectedColor = Color.red;

        if (colorsHighlight.Equals(Color.green))
        {
            colorsHighlight.selectedColor = Color.white;
            buttonClick.GetComponent<Button>().colors = colorsHighlight;
        }
        else
        {
            if (isCorrect == true)
                colorsHighlight.selectedColor = Color.green;
            else
                colorsHighlight.selectedColor = Color.red;

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