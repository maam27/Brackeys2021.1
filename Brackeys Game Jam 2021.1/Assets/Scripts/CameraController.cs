using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera maincam;
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        maincam = Camera.main;
    }

    void Update()
    {
        Quaternion rotation = transform.rotation;
        rotation = new Quaternion(rotation.x, maincam.transform.rotation.y, rotation.z, rotation.w);
        transform.rotation = rotation;
    }

}
