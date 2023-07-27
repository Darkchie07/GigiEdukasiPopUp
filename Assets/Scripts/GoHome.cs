using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoHome : MonoBehaviour
{
    GameObject[] destroyed;
    private void Start()
    {
        destroyed = GameObject.FindGameObjectsWithTag("Instance");
        Destroy(GameObject.FindWithTag("Jawaban"));
        foreach (var a in destroyed)
        {
            Destroy(a);
        }
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("Home");
    }
    public void PostTest()
    {
        Pengetahuan._typetest = "PostTest";
        SceneManager.LoadScene("Pengetahuan");
    }

    public void Materi()
    {
        SceneManager.LoadScene("Materi");
    }

    public void Games()
    {
        SceneManager.LoadScene("Games");
    }

    public void Pemeliharaan()
    {
        SceneManager.LoadScene("PemeliharaanGigi");
    }

    public void Grafik()
    {
        SceneManager.LoadScene("Grafik");
    }
}
