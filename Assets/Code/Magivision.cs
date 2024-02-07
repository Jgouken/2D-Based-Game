using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magivision : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject magivision;
    public float visionSize;

    // Start is called before the first frame update
    void Start()
    {
        visionSize = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (player.GetComponent<Transform>()) magivision.GetComponent<SpriteMask>().enabled = true;
            if (visionSize < player.GetComponent<BoxCollider2D>().size.y * 8)
            {
                visionSize += player.GetComponent<BoxCollider2D>().size.y * 2 / (2 * (visionSize + 1 / player.GetComponent<BoxCollider2D>().size.y * 4));
                magivision.transform.localScale = new Vector3(visionSize, visionSize);
            }
        } else if (visionSize <= 0) magivision.GetComponent<SpriteMask>().enabled = false;

        if (visionSize > 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            visionSize -= player.GetComponent<BoxCollider2D>().size.y * 2 / (2 * (visionSize + 1 / player.GetComponent<BoxCollider2D>().size.y * 4));
            magivision.transform.localScale = new Vector3(visionSize, visionSize);
        }
    }
}
