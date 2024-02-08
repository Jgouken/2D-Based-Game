using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagiCamera : MonoBehaviour
{

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject magiCam;

    // Start is called before the first frame update
    void Update()
    {
        magiCam.GetComponent<Camera>().orthographicSize = mainCamera.GetComponent<Camera>().orthographicSize;
        magiCam.GetComponent<Transform>().position = mainCamera.GetComponent<Transform>().position;
    }
}
