using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pretest : MonoBehaviour
{
    GameObject field;
    GameObject content;

    [System.Serializable]
    public class SoalSikap
    {
        public string soal;
        public Button yes;
        public Button no;
    }

    [Header("Soal")]
    public List<SoalSikap> listSoal = new List<SoalSikap>();

    [Header("Mount")]
    public Button NextButton;
    public Button PrevButton;
    public List<GameObject> listtxtSoal;
    public List<string> listJawaban = new List<string>();
    public List<string> listJawabanBenar = new List<string>();
    public int skor;

    void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Sikap") != null)
            content = GameObject.FindGameObjectWithTag("Sikap").gameObject;
        else if (GameObject.FindGameObjectWithTag("Tindakan") != null)
            content = GameObject.FindGameObjectWithTag("Tindakan").gameObject;
        else if (GameObject.FindGameObjectWithTag("Kontrol") != null)
            content = GameObject.FindGameObjectWithTag("Kontrol").gameObject;

        field = content.transform.GetChild(0).gameObject;
        Transform parent = content.transform;

        GantiSoal();
        DuplicateField(parent);
        SetSoal();
        FillJawaban();
    }

    public void GantiSoal()
    {
        NextButton.onClick.AddListener(() =>
        {
            if (isDone())
            {
                SetJawaban();
                CheckJawaban();
                SceneManager.LoadScene("Home");
            }
            else
                Debug.Log("Masih ada yg kurang");
        });
        PrevButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Pengetahuan");
        });
    }

    #region Set Soal Awal
    public void SetSoal()
    {
        for (int i = 0; i < listtxtSoal.Count; i++)
        {
            listtxtSoal[i].transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = listSoal[i].soal;
        }
    }
    #endregion

    #region SetJawaban

    public void SetJawaban()
    {
        for (int i = 0; i < listtxtSoal.Count; i++)
        {
            var isOn = listtxtSoal[i].gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn;
            if (isOn)
            {
                listJawaban[i] = "1";
            }
            else
            {
                listJawaban[i] = "0";
            }
            Debug.Log(listJawaban[i]);
        }
    }

    public void CheckJawaban()
    {
        for (int i = 0; i < listSoal.Count; i++)
        {
            if (listJawabanBenar[i] == listJawaban[i])
            {
                skor += 1;
            }
        }
        Debug.Log(skor);
    }

    public void FillJawaban()
    {
        for (int i = 0; i < listSoal.Count; i++)
        {
            listJawaban.Add("-");
        }
        for (int i = 0; i < listSoal.Count; i++)
        {
            listJawabanBenar.Add("1");
        }
    }
    #endregion

    public void DuplicateField(Transform parent)
    {
        for (int i = 1; i < listtxtSoal.Count; i++)
        {
            var baris = Instantiate(field, parent);
            listtxtSoal[0] = field;
            listtxtSoal[i] = baris;
        }
    }

    private bool isDone()
    {
        if (listJawaban.Contains("-"))
        {
            return false;
        }
        return true;
    }
}
