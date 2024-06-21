using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// This script should only be applied to the Wizard's "Magivision" object.

public class Magivision : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject magivision;
    [HideInInspector] public List<GameObject> magiforms;
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("/Level").GetComponent<LevelManager>();

        if (levelManager.maximumVisionSize <= 0) levelManager.maximumVisionSize = player.GetComponent<BoxCollider2D>().size.y * 8;
        if (levelManager.visionSpeed <= 0) levelManager.visionSpeed = .2f;

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.layer == 3) magiforms.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && levelManager.visionSize < player.GetComponent<BoxCollider2D>().size.y)
        // Just so the player doesn't seem to hang if standing on a platform
        {
            foreach (GameObject magicPlatform in magiforms)
            {
                magicPlatform.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else if (levelManager.visionSize >= player.GetComponent<BoxCollider2D>().size.y)
        {
            foreach (GameObject magicPlatform in magiforms)
            {
                magicPlatform.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            // EnableMagivision(true);
            if (levelManager.visionSize < levelManager.maximumVisionSize)
            {
                levelManager.visionSize += levelManager.visionSpeed;
                if (levelManager.visionSize > levelManager.maximumVisionSize) levelManager.visionSize = levelManager.maximumVisionSize;
                magivision.transform.localScale = new Vector3(levelManager.visionSize, levelManager.visionSize);
            }
        } // else if (visionSize <= 0) EnableMagivision(false); (Only use if the circle starts actin up, otherwise uneccessary)

        if (levelManager.visionSize > 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            levelManager.visionSize -= levelManager.visionSpeed;
            if (levelManager.visionSize < 0) levelManager.visionSize = 0;
            magivision.transform.localScale = new Vector3(levelManager.visionSize, levelManager.visionSize);
        }
    }
}
