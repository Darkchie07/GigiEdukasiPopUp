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
    }

    [Header("Soal")]
    public List<TBGames> listGambarSoal = new List<TBGames>();

    [Header("Mount")]
    public TMP_Text txtSoal;
    public Button NextButton;
    public Button PrevButton;
    public int idxSoal;

    [Header("Jawaban")]
    public List<string> listJawabanBenar = new List<string>();
    bool isCorrect = false;

    [Header("Button")]
    public List<Button> buttonList;
    private string buttonValue;
    bool isClicked;

    void Start()
    {
        GenerateSoal(idxSoal);
        GantiSoal();
        AddJawaban();
        PrevButton.gameObject.SetActive(false);
    }

    private void GenerateSoal(int idx)
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
                }
                else if (idxSoal == 0)
                {
                    idxSoal += 1;
                    GenerateSoal(idxSoal);
                    PrevButton.gameObject.SetActive(true);
                }
            }
        });

        PrevButton.onClick.AddListener(() =>
        {
            listGambarSoal[idxSoal].gambarSoal.SetActive(false);

            if (idxSoal >= listGambarSoal.Count)
            {
                idxSoal -= 1;
                GenerateSoal(idxSoal);
            }
            else if (2 <= idxSoal && idxSoal < listGambarSoal.Count)
            {
                idxSoal -= 1;
                GenerateSoal(idxSoal);
            }
            else if (idxSoal == 1)
            {
                idxSoal -= 1;
                GenerateSoal(idxSoal);
                PrevButton.gameObject.SetActive(false);
            }
        });
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
                CheckJawaban();
                Debug.Log("Button value " + buttonValue);
            });
        }
    }
    #endregion

    #region Check Jawaban setelah semua soal
    public void CheckJawaban()
    {
        if (buttonValue == listJawabanBenar[idxSoal])
        {
            isCorrect = true;
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
}