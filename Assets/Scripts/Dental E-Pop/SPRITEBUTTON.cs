using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SPRITEBUTTON : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] spriteList;

    void Start()
    {
        //Attach Physics2DRaycaster to the Camera
        Camera.main.gameObject.AddComponent<Physics2DRaycaster>();

        addEventSystem();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < spriteList.Length; i++)
        {
            GameObject sprite = spriteList[i];
            Animation spriteAnim = sprite.GetComponent<Animation>();

            if (sprite.transform.CompareTag("Tekun"))
            {
                spriteAnim.Play("Tekun");
            }
        }
    }

    //Add Event System to the Camera
    void addEventSystem()
    {
        GameObject eventSystem = null;
        GameObject tempObj = GameObject.Find("EventSystem");
        if (tempObj == null)
        {
            eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
        else
        {
            if ((tempObj.GetComponent<EventSystem>()) == null)
            {
                tempObj.AddComponent<EventSystem>();
            }

            if ((tempObj.GetComponent<StandaloneInputModule>()) == null)
            {
                tempObj.AddComponent<StandaloneInputModule>();
            }
        }
    }

}