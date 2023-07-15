using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoHome : MonoBehaviour
{
    public void BackToHome()
    {
        SceneManager.LoadScene("Home");
    }
}
