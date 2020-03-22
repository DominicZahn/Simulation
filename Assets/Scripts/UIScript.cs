using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIScript : MonoBehaviour
{
    public Text fpsText;
    public GameObject pauseMenu;
    private const KeyCode menuKey = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        updateFps();
        if (Input.GetKeyDown(menuKey))
            togglePanel();
    }

    void updateFps()
    {
        fpsText.text = (int)(1.0 / Time.deltaTime) + " FPS";
    }

    void togglePanel()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void resume()
    {
        togglePanel();
    }

    public void options()
    {
        // TODO
    }

    public void menu()
    {
        // TODO
    }

    public void exit()
    {
        Application.Quit();
    }
}
