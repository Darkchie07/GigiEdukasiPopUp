using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using Newtonsoft.Json;
using System.IO;

public class WindowGraph : MonoBehaviour
{
    public static WindowGraph Instance;
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    public List<float> SkorPengetahuan = new List<float>();
    public List<float> SkorSikap = new List<float>();
    public List<float> SkorTindakan = new List<float>();
    public List<float> SkorKontrol = new List<float>();
    public Font arial;

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        LoadData();
        LoadDataKontrol();
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        if (transform.CompareTag("Pengetahuan"))
        {
            List<float> value = SkorPengetahuan;
            ShowGraph(value, 12f, 12, (int _i) => "PostTest " + (_i));
        }
        else if (transform.CompareTag("Sikap"))
        {
            List<float> value = SkorSikap;
            ShowGraph(value, 8f, 8, (int _i) => "PostTest " + (_i));
        }
        else if (transform.CompareTag("Tindakan"))
        {
            List<float> value = SkorTindakan;
            ShowGraph(value, 10f, 10, (int _i) => "PostTest " + (_i));
        }
        else if (transform.CompareTag("Kontrol"))
        {
            List<float> value = SkorKontrol;
            ShowGraph(value, 3f, 3, (int _i) => "PostTest " + (_i));
        }

    }

    private GameObject CreateCircle(Vector2 anchoredPosition, float skor)
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
        if (transform.CompareTag("Kontrol"))
        {
            GameObject text = new GameObject("text");
            text.AddComponent(typeof(Text));
            text.AddComponent(typeof(Canvas));
            gameObject.GetComponent<Canvas>().overrideSorting = true;
            gameObject.GetComponent<Canvas>().sortingOrder = 2;
            text.transform.SetParent(gameObject.transform);
            text.transform.localPosition = new Vector3(0, 25, -9.5f);
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(30, 20);
            textRect.anchorMin = new Vector2(0.5f, 0.5f);
            textRect.anchorMax = new Vector2(0.5f, 0.5f);
            text.GetComponent<Text>().font = arial;
            text.GetComponent<Text>().fontSize = 15;
            text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            text.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
            text.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
            if (skor <= 0.6f)
            {
                text.GetComponent<Text>().text = "Baik";
                text.GetComponent<Text>().color = Color.green;
            }
            else if (skor <= 1.8f)
            {
                text.GetComponent<Text>().text = "Sedang";
                text.GetComponent<Text>().color = Color.yellow;
            }
            else if (skor <= 3f)
            {
                text.GetComponent<Text>().text = "Buruk";
                text.GetComponent<Text>().color = Color.red;
            }
        }
        return gameObject;
    }

    private void ShowGraph(List<float> valueList, float yMaximum, int separatorCount, Func<int, string> getAxisLabelX = null)
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
            float skor = valueList[i];
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), skor);
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

    public void LoadData()
    {
        TestScript.Instance.LoadSkor(false);
        for (int i = 0; i < Jawaban.Instance.skorResponden.Count; i += 3)
        {
            SkorPengetahuan.Add(float.Parse(Jawaban.Instance.skorResponden[i]));
            SkorSikap.Add(float.Parse(Jawaban.Instance.skorResponden[i + 1]));
            SkorTindakan.Add(float.Parse(Jawaban.Instance.skorResponden[i + 2]));
        }
    }

    public void LoadDataKontrol()
    {
        TestScript.Instance.LoadSkor(true);
        for (int i = 0; i < Debris.Instance.skorKontrolResponden.Count; i += 2)
        {
            var skor = (float)Debris.Instance.skorKontrolResponden[i + 1] / (float)Debris.Instance.skorKontrolResponden[i];
            SkorKontrol.Add(skor);
            Debug.Log(skor);
        }
    }
}

