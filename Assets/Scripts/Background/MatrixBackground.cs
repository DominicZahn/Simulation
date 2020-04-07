using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatrixBackground : MonoBehaviour
{
    public TextMeshProUGUI background;
    TextGrid tg;
    const int lineCount = 1;
    const int maxColumns = 68;
    const int maxRows = 172;
    const int maxLength = 20;

    // Start is called before the first frame update
    void Start()
    {
        background.richText = true;
        tg = new TextGrid(maxRows, maxColumns);

        InvokeRepeating("updateMatrix", 0, 1f);
        /*
        background.richText = true;
        string oldText = background.text;
        background.text = $"Coloured text <color=red>{ oldText }</color>";
        */
    }

    void updateMatrix()
    {
        tg.expandLines(lineCount, maxLength);
        background.text = tg.gridToString();
    }

    // Update is called once per frame
    void Update()
    {
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
