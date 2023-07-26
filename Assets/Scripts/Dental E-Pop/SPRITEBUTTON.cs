using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SPRITEBUTTON : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] spriteList;
    public AnimationClip[] clipAnimation;
    public bool isAnim;

    void Start()
    {
        //Attach Physics2DRaycaster to the Camera
        Screen.orientation = ScreenOrientation.LandscapeRight;
        Camera.main.gameObject.AddComponent<Physics2DRaycaster>();

        addEventSystem();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObject.transform.CompareTag("Dental") && gameObject.GetComponent<SPRITEBUTTON>().isAnim == false)
        {
            Debug.Log("Masok");
            var reverseAnim = gameObject.GetComponent<Animation>().clip.name;
            gameObject.GetComponent<Animation>()[reverseAnim].speed = 1;
            gameObject.GetComponent<Animation>().Play();
            gameObject.GetComponent<SPRITEBUTTON>().isAnim = true;
        }
        else if (gameObject.transform.CompareTag("Dental") && gameObject.GetComponent<SPRITEBUTTON>().isAnim == true)
        {
            var reverseAnim = gameObject.GetComponent<Animation>().clip.name;
            gameObject.GetComponent<Animation>()[reverseAnim].time = gameObject.GetComponent<Animation>()[reverseAnim].length;
            gameObject.GetComponent<Animation>()[reverseAnim].speed = -1;
            gameObject.GetComponent<Animation>().Play();
            gameObject.GetComponent<SPRITEBUTTON>().isAnim = false;
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