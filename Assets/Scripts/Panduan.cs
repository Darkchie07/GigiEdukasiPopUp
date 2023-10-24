using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panduan : MonoBehaviour
{
    public Image PanelPanduan;
    public List<Sprite> listPanduan;
    public TMP_Text counter;
    public TMP_Text maxCounter;

    public int index;
    // Start is called before the first frame update
    void Start()
    {
        maxCounter.text = listPanduan.Count.ToString();
    }

    public void OnEnable()
    {
        index = 0;
        PanelPanduan.sprite = listPanduan[index];
    }

    public void Next()
    {
        index += 1;
        if (index >= listPanduan.Count)
        {
            index = 0;
        }

        PanelPanduan.sprite = listPanduan[index];
        counter.text = (index + 1).ToString();
    }
    
    public void Before()
    {
        index -= 1;
        if (index <= -1)
        {
            index = listPanduan.Count - 1;
        }

        PanelPanduan.sprite = listPanduan[index];
        counter.text = (index + 1).ToString();
    }
}
