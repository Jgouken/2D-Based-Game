using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject wizardPrefab;
    public GameObject bardPrefab;
    public GameObject roguePrefab;

    // ROGUE
    public float maximumArrows = 10f;
    public List<GameObject> arrowCount = new List<GameObject>();

    // WIZARD
    public float visionSize = 0f;
    public float maximumVisionSize; // This value should only be set level-per-level, not manually. If kept unset (0), it will default to 8 times the player's hitbox size.
    public float visionSpeed;
    
    // BARD
    public GameObject left;
    public GameObject right;
    public GameObject down;
    public GameObject up;
    public List<GameObject> arrowObjects = new List<GameObject>();
    public string arrowCode = "";
    public string submittedCode = "";

    private GameObject prefb;

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
                        prefb = Instantiate(wizardPrefab, charact.transform.parent.position, Quaternion.identity);
                    }
                    else if (charact.transform.parent.gameObject.name.StartsWith("Wizard"))
                    {
                        prefb = Instantiate(roguePrefab, charact.transform.parent.position, Quaternion.identity);
                    }
                    else if (charact.transform.parent.gameObject.name.StartsWith("Rogue"))
                    {
                        prefb = Instantiate(bardPrefab, charact.transform.parent.position, Quaternion.identity);
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