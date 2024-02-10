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
    public float visionSize;
    public float maximumVisionSize; // This value should only be set level-per-level, not manually. If kept unset (0), it will default to 8 times the player's hitbox size.

    // Start is called before the first frame update
    void Start()
    {
        if (maximumVisionSize <= 0) maximumVisionSize = player.GetComponent<BoxCollider2D>().size.y * 8;
        visionSize = 0;

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects) {
            if (go.layer == 3) magiforms.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && visionSize < player.GetComponent<BoxCollider2D>().size.y)
        // Just so the player doesn't seem to hang if standing on a platform
        {
            foreach (GameObject magicPlatform in magiforms)
            {
                magicPlatform.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else if (visionSize >= player.GetComponent<BoxCollider2D>().size.y)
        {
            foreach (GameObject magicPlatform in magiforms)
            {
                magicPlatform.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            // EnableMagivision(true);
            if (visionSize < maximumVisionSize)
            {
                visionSize += player.GetComponent<BoxCollider2D>().size.y * 2 / (2 * (visionSize + 1 / player.GetComponent<BoxCollider2D>().size.y * 4));
                if (visionSize > maximumVisionSize) visionSize = maximumVisionSize;
                magivision.transform.localScale = new Vector3(visionSize, visionSize);
            }
        } // else if (visionSize <= 0) EnableMagivision(false); (Only use if the circle starts actin up, otherwise uneccessary)

        if (visionSize > 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            visionSize -= player.GetComponent<BoxCollider2D>().size.y * 2 / (2 * (visionSize + 1 / player.GetComponent<BoxCollider2D>().size.y * 4));
            if (visionSize < 0) visionSize = 0;
            magivision.transform.localScale = new Vector3(visionSize, visionSize);
        }
    }

    void EnableMagivision(Boolean enabled)
    {
        magivision.GetComponent<SpriteMask>().enabled = enabled;
        magivision.GetComponent<SpriteRenderer>().enabled = enabled;
    }
}
