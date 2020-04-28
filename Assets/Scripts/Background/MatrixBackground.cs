using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SysColor = System.Drawing.Color;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;
using SysRandom = System.Random;
using URandom = UnityEngine.Random;
using UnityEngine.Events;

public class MatrixBackground : MonoBehaviour
{
    public TextMeshProUGUI background;
    public GameObject panel;
    TextGrid tg;

    const int lineCount = 150;
    const int maxColumns = 68;
    const int maxRows = 170;
    const int maxLength = 50;

    // Start is called before the first frame update
    void Start()
    {
        background.richText = true;
        tg = new TextGrid(maxRows, maxColumns);
        tg.setupLines(lineCount, maxLength);

        addButton(tg, "Button", 15, 20, 100, 30, SysColor.Red, onClick);

        InvokeRepeating("updateMatrix", 0, 0.1f);


        /*
        background.richText = true;
        string oldText = "Hello world";
        background.text = $"Colored text <color=red>{ oldText }</color>";
        */
    }

    void updateMatrix()
    {
        tg.expandLines();
        background.text = tg.gridToString();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void addButton(TextGrid tg, string text, int width, int height, int x, int y, SysColor color, UnityAction onClick)
    {
        const int standardDelta = 1;
        tg.addButton(text, standardDelta, new System.Drawing.Size(width, height), new System.Drawing.Point(x, y), color);

        GameObject nButton = new GameObject();
        nButton.AddComponent<RectTransform>();
        nButton.transform.SetParent(panel.transform);
        nButton.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        nButton.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        nButton.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
        nButton.GetComponent<RectTransform>().anchoredPosition = posButtonUnity(x, y);
        nButton.GetComponent<RectTransform>().sizeDelta = new Vector2(width * 10.6f, height * 15.25f);
        nButton.AddComponent<Button>();
        nButton.AddComponent<Image>();
        nButton.GetComponent<Image>().color = Color.clear;
        nButton.GetComponent<Button>().onClick.AddListener(onClick);
    }

    private Vector2 posButtonUnity(int x, int y)
    {
        
        return new Vector2(11.2f * x + 16, -15.8f * y - 7);
    }

    void onClick()
    {
        Debug.Log("Clicked");
        addButton(tg, "Button", URandom.Range(8, 20), URandom.Range(6, 20), URandom.Range(0, tg.getRows() - 20), URandom.Range(0, tg.getColumns() - 20), SysColor.Red, onClick);
    }

    /// <summary>
    /// converts a integer number (0 to 9) to the char (0 to 9)
    /// </summary>
    /// <param name="number">(0 to 9)</param>
    /// <returns>a char (0 to 9)</returns>
    private char intToCharNumbers(int number)
    {
        if (number < 0 || number > 9)
            throw new System.ArgumentOutOfRangeException("only numbers form 0 to 9");
        return (char)(48 + number);
    }
}
