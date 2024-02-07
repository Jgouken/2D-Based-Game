using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPlatforms : MonoBehaviour
{

    [SerializeField] private GameObject magicPlatform;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Magivision magivision;

    // Update is called once per frame

    void Start() {
        if (player.playerClass != 'W' || !magivision) Destroy(magicPlatform);
    }

    void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && magivision.visionSize < 0) {
            magicPlatform.GetComponent<BoxCollider2D>().enabled = false;
        } else {
            magicPlatform.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
