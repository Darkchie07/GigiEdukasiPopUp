using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Materi : MonoBehaviour
{
    [Header("Soal")]
    public List<GameObject> listMateri = new List<GameObject>();
    public GameObject[] listImages;

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
        PrevButton.gameObject.SetActive(false);
        checkButton();
    }

    // private void Update()
    // {
    //     if (checkButton())
    //     {
    //         NextButton.enabled = true;
    //     }
    // }

    public bool checkButton()
    {
        if (listMateri[idxSoal].activeSelf)
        {
            listImages = GameObject.FindGameObjectsWithTag("Dental");

            // if (listImages.Length == 0)
            //     NextButton.enabled = true;
            // else
            // {
            //     NextButton.enabled = false;

            for (int i = 0; i < listImages.Length; i++)
            {
                if (listImages[i].GetComponent<SPRITEBUTTON>().isClicked == true)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            // }
        }
        return true;
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
            if (checkButton())
            {
                listMateri[idxSoal].SetActive(false);

                if (idxSoal >= listMateri.Count - 1)
                {
                    // if (isDone())
                    // {
                    //     CheckJawaban();
                    Screen.orientation = ScreenOrientation.Portrait;
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
            }
            else
            {
                for (int i = 0; i < listImages.Length; i++)
                {
                    if (listImages[i].GetComponent<SPRITEBUTTON>().isClicked == false)
                        StartCoroutine(TimerCoroutine(i));
                }
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

    IEnumerator TimerCoroutine(int i)
    {
        listImages[i].GetComponent<SpriteRenderer>().color = new Color(1, 0.4117647058823529f, 0.4117647058823529f);
        yield return new WaitForSeconds(0.5f);
        listImages[i].GetComponent<SpriteRenderer>().color = Color.white;
    }
}