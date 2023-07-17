using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwipeTindakan : MonoBehaviour
{
    public TMP_Text title;
    public GameObject scrollBar;
    float scrollPos = 0;
    float[] pos;
    void Update()
    {
        // SnapTo(contentPanel);
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        if (Input.GetMouseButton(0))
        {
            scrollPos = scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scrollPos < pos[i] && scrollPos > pos[i])
                {
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scrollPos < pos[i] && scrollPos > pos[i])
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.1f, 1.1f), 0.1f);
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(1f, 1f), 0.1f);
                    }
                }
            }
        }

        if (scrollPos == 0f || scrollPos >= 0.5455283f)
            title.text = "Persiapan";
        else if (scrollPos <= (-0.14f))
            title.text = "Setelah menggosok gigi";
        else if (scrollPos <= 0.5455283f)
            title.text = "Pelaksanaan";

        if (this.transform.localPosition.y <= 0)
        {
            this.transform.localPosition = Vector3.zero;
        }
        else if (this.transform.localPosition.y >= 3250)
        {
            this.transform.localPosition = new Vector3(0, 3250, 0);
        }
    }
}
