using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLogic : MonoBehaviour
{
    [SerializeField] private LayerMask[] groundLayers;
    [SerializeField] private Transform groundCheck;
    public float speed = 4f;
    private Vector3 screenPos;
    private Vector3 posRelative;
    private float trueAngle;
    private GameObject level;
    private GameObject player;

    void Start()
    {
        level = GameObject.Find("/Level");
        player = GameObject.Find("Player");

        screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posRelative = new Vector3(screenPos.x - player.transform.position.x, screenPos.y - player.transform.position.y);
        trueAngle = (float)Math.Atan2(screenPos.y - player.transform.position.y, screenPos.x - player.transform.position.x);

        transform.parent = level.transform;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = player.transform.position;

        if (trueAngle < 0) trueAngle += 6;
        trueAngle *= 60;

        transform.eulerAngles = new Vector3(0, 0, trueAngle);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(posRelative.x * speed, posRelative.y * speed);
    }

    void OnCollisionEnter2D(Collision2D collide)
    {
        for (int i = 0; i < groundLayers.Length; i++)
        {
            if (collide.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast") && Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayers[i]))
            {
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.GetComponent<BoxCollider2D>().excludeLayers = 0;
                return;
            }
        }
    }
}
