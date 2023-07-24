using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;

    public List<double> SkorPengetahuan = new List<double>();
    public List<double> SkorSikap = new List<double>();
    public List<double> SkorTindakan = new List<double>();
    public List<int> SkorKontrol = new List<int>();

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        if (transform.CompareTag("Pengetahuan"))
        {
            List<float> value = new List<float>() { 5.64f, 8, 8, 6, 8 };
            ShowGraph(value, 12f, (int _i) => "PostTest " + (_i));
        }
        else if (transform.CompareTag("Sikap"))
        {
            List<float> value = new List<float>() { 5.64f, 8, 8, 6, 8 };
            ShowGraph(value, 8f, (int _i) => "PostTest " + (_i));
        }
        else if (transform.CompareTag("Tindakan"))
        {
            List<float> value = new List<float>() { 5.64f, 8, 8, 6, 8 };
            ShowGraph(value, 10f, (int _i) => "PostTest " + (_i));
        }
        else if (transform.CompareTag("Kontrol"))
        {
            List<float> value = new List<float>() { 3, 0, 2, 0, 1 };
            ShowGraph(value, 3f, (int _i) => "PostTest " + (_i));
        }

    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.AddComponent(typeof(Canvas));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Canvas>().overrideSorting = true;
        gameObject.GetComponent<Canvas>().sortingOrder = 1;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(20, 20);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<float> valueList, float yMaximum, Func<int, string> getAxisLabelX = null)
    {
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        float graphHeight = graphContainer.sizeDelta.y;
        float xSize = 50f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -27f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            if (i == 0)
                labelX.GetComponent<Text>().text = "PreTest";

            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(xPosition, 0);
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-13f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMaximum).ToString();

            if (i != 0)
            {
                RectTransform dashX = Instantiate(dashTemplateX);
                dashX.SetParent(graphContainer, false);
                dashX.gameObject.SetActive(true);
                dashX.anchoredPosition = new Vector2(0, normalizedValue * graphHeight);
            }
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("connection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(0, 196, 204);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 5f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

    public void loadSkor()
    {
        Jawaban.Instance.LoadSkor();
        for (int i = 0; i < Jawaban.Instance.skorResponden.Count; i += 3)
        {
            SkorPengetahuan.Add(Convert.ToDouble(Jawaban.Instance.skorResponden[i]));
            SkorSikap.Add(Convert.ToDouble(Jawaban.Instance.skorResponden[i + 1]));
            SkorTindakan.Add(Convert.ToDouble(Jawaban.Instance.skorResponden[i + 2]));
        }
    }


}
