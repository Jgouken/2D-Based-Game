using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magivision : MonoBehaviour
{
    [SerializeField] private GameObject magivision;
    [SerializeField] private BoxCollider2D playerCollider;
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
            magivision.GetComponent<SpriteMask>().enabled = true;
            if (visionSize < playerCollider.size.y * 8)
            {
                visionSize += playerCollider.size.y / (5 * (visionSize + 1 / playerCollider.size.y));
                magivision.transform.localScale = new Vector3(visionSize, visionSize);
            }
        } else if (visionSize <= 0) magivision.GetComponent<SpriteMask>().enabled = false;

        if (visionSize > 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            visionSize -= playerCollider.size.y / 15;
            magivision.transform.localScale = new Vector3(visionSize, visionSize);
        }
    }
}
