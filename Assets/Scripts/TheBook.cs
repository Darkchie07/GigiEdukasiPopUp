using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBook : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        // Screen.orientation = ScreenOrientation.LandscapeRight;
        anim = GetComponent<Animator>();
        anim.Play("TheBook");
    }

    void Update()
    {

    }
}
