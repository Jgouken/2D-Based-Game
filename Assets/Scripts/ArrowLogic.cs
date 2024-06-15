using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLogic : MonoBehaviour
{
    [SerializeField] private LayerMask[] groundLayers;
    [SerializeField] private Transform groundCheck;
    public float speed = 4f;
    public float maximumArrows = 10f;
    public bool moving = true;
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
        trueAngle = Mathf.Atan2(screenPos.y - player.transform.position.y, screenPos.x - player.transform.position.x);

        transform.parent = level.transform;
        transform.localScale = new Vector3(.5f, .5f, .5f);
        transform.position = player.transform.position;

        if (trueAngle < 0) trueAngle += 6;
        trueAngle *= 60;

        transform.eulerAngles = new Vector3(0, 0, trueAngle);
        GetComponent<Rigidbody2D>().velocity = new Vector2(posRelative.x * speed, posRelative.y * speed);
    }

    void OnCollisionEnter2D(Collision2D collide)
    {
        for (int i = 0; i < groundLayers.Length; i++)
        {
            if (collide.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast") && Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayers[i]))
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.GetComponent<BoxCollider2D>().excludeLayers = 0;
                moving = false;
                return;
            }

            if (collide.gameObject.layer == LayerMask.NameToLayer("Arrow")) {
                player.GetComponent<RogueMovement>().arrowCount.Remove(collide.gameObject);
                if (collide.transform.childCount > 1) player.transform.parent = level.transform.Find("rogue");
                Destroy(collide.gameObject);
            }
        }
    }

    void Update()
    {
        if (player.GetComponent<RogueMovement>().arrowCount.Count > maximumArrows && player.GetComponent<RogueMovement>().arrowCount.Contains(gameObject))
        {
            // I image a fade out then destroy
            player.GetComponent<RogueMovement>().arrowCount.Remove(gameObject);
            if (transform.childCount > 1) player.transform.parent = level.transform.Find("rogue");
            Destroy(gameObject);
        }
        if (!moving) return;
        trueAngle = (float)Math.Atan2(screenPos.y - player.transform.position.y, screenPos.x - player.transform.position.x);
        //var tempAngle = new Quaternion();
        transform.rotation = LookAtTarget(GetComponent<Rigidbody2D>().velocity);
    }

    public static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }
}
