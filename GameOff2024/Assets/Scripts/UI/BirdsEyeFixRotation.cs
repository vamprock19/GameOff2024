using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsEyeFixRotation : MonoBehaviour
{
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    //fix the rotation of an object
    void LateUpdate()
    {
        transform.eulerAngles = new Vector3(90, playerCamera.transform.eulerAngles.y, 0);
    }
}
