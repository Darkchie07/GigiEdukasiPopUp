using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Materi : MonoBehaviour
{
    [Header("Soal")]
    public List<GameObject> listMateri = new List<GameObject>();

    [Header("Mount")]
    public Button NextButton;
    public Button PrevButton;
    public int idxSoal;

    [Header("Animation")]
    Animator anim;
    public string[] animList;
    public GameObject bookAnim;

    void Start()
    {
        anim = bookAnim.GetComponent<Animator>();
        GenerateSoal(idxSoal);
        GantiSoal();
    }

    public void GenerateSoal(int idx)
    {
        listMateri[idx].SetActive(true);
        anim.Play(animList[idx]);
    }

    #region Tombol Next and Previous
    public void GantiSoal()
    {
        NextButton.onClick.AddListener(() =>
        {
            listMateri[idxSoal].SetActive(false);

            if (idxSoal >= listMateri.Count - 1)
            {
                // if (isDone())
                // {
                //     CheckJawaban();
                SceneManager.LoadScene("Home");
                // }
            }
            else if (0 < idxSoal && idxSoal < listMateri.Count)
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
        });

        PrevButton.onClick.AddListener(() =>
        {
            listMateri[idxSoal].SetActive(false);

            if (idxSoal >= listMateri.Count)
            {
                idxSoal -= 1;
                GenerateSoal(idxSoal);
            }
            else if (2 <= idxSoal && idxSoal < listMateri.Count)
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
}