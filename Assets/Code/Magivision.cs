using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magivision : MonoBehaviour
{
    [SerializeField] private GameObject magivision;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private float visionSize;

    // Start is called before the first frame update
    void Start()
    {
        visionSize = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            magivision.GetComponent<SpriteRenderer>().enabled = true;
            if (visionSize < playerCollider.size.y * 8)
            {
                visionSize += playerCollider.size.y / 15;
                magivision.transform.localScale = new Vector3(visionSize, visionSize);
            }
        } else if (visionSize <= 0) magivision.GetComponent<SpriteRenderer>().enabled = false;

        if (visionSize > 0 && !Input.GetMouseButton(1))
        {
            visionSize -= playerCollider.size.y / 15;
            magivision.transform.localScale = new Vector3(visionSize, visionSize);
        }
    }
}
