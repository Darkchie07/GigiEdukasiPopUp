using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sikap : MonoBehaviour
{
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
    public List<TMP_Text> listtxtSoal = new List<TMP_Text>();
    
    public List<string> listJawaban = new List<string>();
    public List<string> listJawabanBenar = new List<string>();
    public int skor;
    // Start is called before the first frame update
    void Start()
    {
        SetSoal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Set Soal Awal
    public void SetSoal()
    {
        for (int i = 0; i < listtxtSoal.Count; i++)
        {
            listtxtSoal[i].text = listSoal[i].soal;
        }
    }
    #endregion

    #region SetJawaban & Ubah Jawaban
    
    

    #endregion
}
