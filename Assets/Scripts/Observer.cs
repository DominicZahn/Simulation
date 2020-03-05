using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float turnSpeedH = 1f;
    public float turnSpeedV = 1f;

    private float yaw = 0f;
    private float pitch = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        turn();
    }

    private void turn()
    {
        const float maxPitch = 90;

        yaw += turnSpeedH * Input.GetAxis("Mouse X");
        pitch -= turnSpeedV * Input.GetAxis("Mouse Y");

        if (pitch < -maxPitch)
            pitch = -maxPitch;
        else if (pitch > maxPitch)
            pitch = maxPitch;

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    private void movement()
    {
        //x | z
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //y
        float height = 0;
        if (Input.GetKey(KeyCode.LeftControl) && transform.position.y > 1) //Ctrl down and not under the ground
        {
            height = -1;
        } else if (Input.GetKey(KeyCode.Space))
        {
            height = 1;
        }
        //execute movement
        if (horizontal != 0 || vertical != 0 || height != 0)
        {
            transform.position += moveSpeed * calcCoor(new Vector3(horizontal, height, vertical), yaw);
        }
    }

    //transforms the movement vector into the global movement vector
    private Vector3 calcCoor(Vector3 vOld, float angle)
    {
        //old vertical
        float nHorizontal = vOld.x * Mathf.Sin(radianToDegree(angle + 90));
        float nVertical = vOld.x * Mathf.Cos(radianToDegree(angle + 90));
        //old horizontal
        nHorizontal += vOld.z * Mathf.Sin(radianToDegree(angle));
        nVertical += vOld.z * Mathf.Cos(radianToDegree(angle));
        return new Vector3(nHorizontal, vOld.y, nVertical);
    }

    private float radianToDegree(float degree)
    {
        return (Mathf.PI * (degree / 180));
    }
}
