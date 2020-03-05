using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotScript : MonoBehaviour
{
    public Camera Observer, ChaseCamera;
    public bool chaseCameraOn = true;
    public KeyCode switchKey;

    // Start is called before the first frame update
    void Start()
    {
        //set active camera
        Observer.gameObject.SetActive(!chaseCameraOn);
        ChaseCamera.gameObject.SetActive(chaseCameraOn);
        Cursor.visible = chaseCameraOn;
    }

    // Update is called once per frame
    void Update()
    {
        switchCamera();
    }

    //switches between the observer and chaseCamera
    void switchCamera()
    {
        if (Input.GetKeyDown(switchKey))
        {
            chaseCameraOn = !chaseCameraOn;
            Cursor.visible = chaseCameraOn;
            Observer.gameObject.SetActive(!chaseCameraOn);
            ChaseCamera.gameObject.SetActive(chaseCameraOn);
        }
    }
}
