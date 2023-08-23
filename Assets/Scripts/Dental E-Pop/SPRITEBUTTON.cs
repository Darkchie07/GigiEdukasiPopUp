using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SPRITEBUTTON : MonoBehaviour, IPointerClickHandler
{
    public Materi materiManager;
    public bool isAnim;
    public bool isClicked;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        addEventSystem();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var reverseAnim = gameObject.GetComponent<Animation>().clip.name;

        if (gameObject.transform.CompareTag("Dental") && gameObject.GetComponent<SPRITEBUTTON>().isAnim == false)
        {
            gameObject.GetComponent<Animation>()[reverseAnim].speed = 2;
            gameObject.GetComponent<Animation>().Play();
            StartCoroutine(TimerCoroutine());
            gameObject.GetComponent<SPRITEBUTTON>().isAnim = true;
            gameObject.GetComponent<SPRITEBUTTON>().isClicked = true;
        }
        else if (gameObject.transform.CompareTag("Dental") && gameObject.GetComponent<SPRITEBUTTON>().isAnim == true)
        {
            gameObject.GetComponent<Animation>()[reverseAnim].time = gameObject.GetComponent<Animation>()[reverseAnim].length;
            gameObject.GetComponent<Animation>()[reverseAnim].speed = -2;
            gameObject.GetComponent<Animation>().Play();
            StartCoroutine(TimerCoroutine());
            gameObject.GetComponent<SPRITEBUTTON>().isAnim = false;
        }
    }

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

    IEnumerator TimerCoroutine()
    {
        materiManager.NextButton.enabled = false;
        materiManager.PrevButton.enabled = false;

        yield return new WaitForSeconds(0.5f);

        materiManager.NextButton.enabled = true;
        materiManager.PrevButton.enabled = true;
    }
}