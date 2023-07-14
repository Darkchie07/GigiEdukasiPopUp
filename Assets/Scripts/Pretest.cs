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
        // public Button yes;
        // public Button no;
    }

    [Header("Soal")]
    public List<SoalSikap> listSoal = new List<SoalSikap>();

    public GameObject[] yes;
    public GameObject[] no;

    [Header("Mount")]
    public Button NextButton;
    public Button PrevButton;
    public List<GameObject> listtxtSoal;
    public List<string> listJawaban = new List<string>();
    public List<int> listJawabanYa = new List<int>();
    public List<int> listJawabanNo = new List<int>();
    public int Answered;
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
        yes = GameObject.FindGameObjectsWithTag("Ya");
        no = GameObject.FindGameObjectsWithTag("Tidak");
        Debug.Log(listSoal.Count);
        Debug.Log(yes.Length);
        SetSoal();
        // FillJawaban();
    }

    public void GantiSoal()
    {
        NextButton.onClick.AddListener(() =>
        {
            if (isDone())
            {
                FillJawaban();
                Debug.Log(skor);
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

    public void FillJawaban()
    {
        for (int i = 0; i < listJawabanNo.Count; i++)
        {
            listJawaban[listJawabanNo[i]] = "1";
        }
        for (int i = 0; i < listJawabanYa.Count; i++)
        {
            listJawaban[listJawabanYa[i]] = "0";
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
}
