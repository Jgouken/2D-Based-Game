using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject wizardPrefab;
    public GameObject bardPrefab;

    private GameObject prefb = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Changed Character!");
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject charact in allObjects)
            {
                if (charact.name == "Player")
                {
                    if (charact.transform.parent.gameObject.name.StartsWith("Bard"))
                    {
                        prefb = Instantiate(wizardPrefab, charact.transform.position, Quaternion.identity);
                    }
                    else if (charact.transform.parent.gameObject.name.StartsWith("Wizard"))
                    {
                        prefb = Instantiate(bardPrefab, charact.transform.position, Quaternion.identity);
                    }

                    foreach (GameObject cam in GameObject.FindGameObjectsWithTag("Camera"))
                    {
                        if (cam.GetComponent<GrabPlayer>() != null)
                        {
                            cam.GetComponent<GrabPlayer>().player = prefb.transform.GetChild(0).gameObject;
                            cam.GetComponent<CinemachineVirtualCamera>().Follow = prefb.transform.GetChild(0).gameObject.transform;
                        }
                    }
                    Destroy(charact.transform.parent.gameObject);
                }
            }
        }
    }
}