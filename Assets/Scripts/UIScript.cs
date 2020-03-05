using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIScript : MonoBehaviour
{
    public Text fpsText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updateFps();
    }

    void updateFps()
    {
        fpsText.text = (int)(1.0 / Time.deltaTime) + " FPS";
    }
}
