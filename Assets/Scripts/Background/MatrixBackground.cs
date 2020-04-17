using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SysColor = System.Drawing.Color;

public class MatrixBackground : MonoBehaviour
{
    public TextMeshProUGUI background;
    TextGrid tg;
    const int lineCount = 150;
    const int maxColumns = 68;
    const int maxRows = 171;
    const int maxLength = 50;

    // Start is called before the first frame update
    void Start()
    {
        background.richText = true;
        tg = new TextGrid(maxRows, maxColumns);
        tg.setupLines(lineCount, maxLength);

        tg.addText(50, 50, "Hello world. My name is Dominic and i hope this works with a longer sentence.", SysColor.Red);
        tg.addText(0, 0, "HELLO", SysColor.Transparent);
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
        //updateMatrix();
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
