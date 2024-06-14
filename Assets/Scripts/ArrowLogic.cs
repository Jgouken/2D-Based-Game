using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLogic : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private GameObject level;
    [SerializeField] private GameObject player;
    public float speed = 4f;
    public float angle = 0f;

    void Start()
    {
        level = GameObject.Find("/Level");
        player = GameObject.Find("Player");

        var screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var posRelative = new Vector3(screenPos.x - player.transform.position.x, screenPos.y - player.transform.position.y);
        var trueAngle = (float)Math.Atan2(screenPos.y - player.transform.position.y, screenPos.x - player.transform.position.x);

        transform.parent = level.transform;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = new Vector3(player.transform.position.x + (boxCollider2D.size.x * player.transform.localScale.x), player.transform.position.y + boxCollider2D.size.y);

        if (trueAngle < 0) trueAngle += 6;
        trueAngle *= 60;

        transform.eulerAngles = new Vector3(0, 0, trueAngle);
    }

    void Update()
    {
        // var screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // var posRelative = new Vector3(screenPos.x - player.transform.position.x, screenPos.y - player.transform.position.y);
        // Debug.DrawRay(player.transform.position, posRelative, Color.red);
        // Debug.Log(trueAngle);
    }
}
