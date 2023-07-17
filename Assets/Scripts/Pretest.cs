using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pretest : MonoBehaviour
{
    // public static GameObject JawabanInstance;
    public GameObject jawaban;
    
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
    public GameObject[] drop;

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
    private bool isSikap;
    private bool isTindakan;

    [Header("Kontrol")]
    public List<Sprite> image = new List<Sprite>();
    public GameObject totalText;
    public int nilai;
    public List<int> listnilai = new List<int>();

    void Awake()
    {
        jawaban = GameObject.FindGameObjectWithTag("Jawaban");
        if (GameObject.FindGameObjectWithTag("Sikap") != null)
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
            content = GameObject.FindGameObjectWithTag("Kontrol").gameObject;

        field = content.transform.GetChild(0).gameObject;
        Transform parent = content.transform;

        DuplicateField(parent);
        yes = GameObject.FindGameObjectsWithTag("Ya");
        no = GameObject.FindGameObjectsWithTag("Tidak");
        drop = GameObject.FindGameObjectsWithTag("Dropdown");
        GantiSoal();
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
                if (isTindakan)
                {
                    jawaban.GetComponent<Jawaban>().UploadDataToDrive();
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
        if (content.CompareTag("Kontrol"))
        {
            for (int i = 0; i < drop.Length; i++)
            {
                drop[i].GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate {changeSkor();});
            }
        }
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
        for (int i = 0; i < listtxtSoal.Count; i++)
        {
            int no = i + 1;
            listtxtSoal[i].transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = no.ToString();
            listtxtSoal[i].transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = listSoal[i].soal;
        }

        if (content.CompareTag("Kontrol"))
        {
            for (int i = 0; i < listtxtSoal.Count; i++)
                listtxtSoal[i].transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Image>().sprite = image[i];
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
            jawaban.GetComponent<Jawaban>().jawabanSikap = listJawaban;
        }else if (isTindakan)
        {
            jawaban.GetComponent<Jawaban>().jawabanTindakan = listJawaban;
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
