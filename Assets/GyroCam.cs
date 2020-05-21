using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCam : MonoBehaviour
{
    private Gyroscope gyroscope;
    private bool gyroSupported;
    private Quaternion rotfix;

    [SerializeField]
    private Transform worldObj;
    private float startY;
    void Start()
    {
        gyroSupported = SystemInfo.supportsGyroscope;

        GameObject camParent = new GameObject("camParent");
        camParent.transform.position = transform.position;
        transform.parent = camParent.transform; 

        if (gyroSupported)
        {
            gyroscope = Input.gyro;
            gyroscope.enabled = true;

            camParent.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
            rotfix = new Quaternion(0, 0, 1, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gyroSupported && startY == 0)
        {
            ResetGyroRotation();
        }
        transform.localRotation = gyroscope.attitude * rotfix;
    }

    void ResetGyroRotation()
    {
        startY = transform.eulerAngles.y;
        worldObj.rotation = Quaternion.Euler(0f, startY, 0f);
    }
}
