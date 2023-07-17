using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeTindakan : MonoBehaviour
{
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
                if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
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
    }


    // protected ScrollRect scrollRect;
    // protected RectTransform contentPanel;

    // public void SnapTo(RectTransform target)
    // {
    //     Canvas.ForceUpdateCanvases();

    //     contentPanel.anchoredPosition =
    //             (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
    //             - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    // }
}
