using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnim : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Animation>().Play();
    }
}
