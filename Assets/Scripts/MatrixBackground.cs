using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatrixBackground : MonoBehaviour
{
    public TextMeshProUGUI background;

    // Start is called before the first frame update
    void Start()
    {
        background.richText = true;
        string oldText = background.text;
        background.text = $"Coloured text <color=red>{ oldText }</color>";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
