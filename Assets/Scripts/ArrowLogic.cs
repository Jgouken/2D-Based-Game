using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLogic : MonoBehaviour
{
    [SerializeField] private LayerMask[] groundLayers;
    [SerializeField] private Transform groundCheck;
    private float destroid = 0f;
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

        GetComponent<Rigidbody2D>().velocity = new Vector2(posRelative.x, posRelative.y);
    }

    void OnCollisionEnter2D(Collision2D collide)
    {
        for (int i = 0; i < groundLayers.Length; i++)
        {
            if (collide.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast") && Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayers[i]))
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.GetComponent<BoxCollider2D>().excludeLayers = 0;
                return;
            }

            if (collide.gameObject.layer == LayerMask.NameToLayer("Arrow") && collide.gameObject.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            {
                player.GetComponent<RogueMovement>().arrowCount.Remove(collide.gameObject);
                if (collide.transform.childCount > 1) player.transform.parent = level.transform.Find("Rogue");
                Destroy(collide.gameObject);
            }
        }
    }

    void Update()
    {
        if (destroid != 0f)
        {
            if ((Time.time - destroid) > 3)
            {
                if (transform.childCount > 1) player.transform.parent = level.transform.Find("Rogue");
                Destroy(gameObject);
            }
            return;
        }
        else
        {
            try
            {
                if (player.GetComponent<RogueMovement>().arrowCount.Count > player.GetComponent<RogueMovement>().maximumArrows && player.GetComponent<RogueMovement>().arrowCount.Contains(gameObject))
                {
                    player.GetComponent<RogueMovement>().arrowCount.Remove(gameObject);
                    gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .25f);
                    destroid = Time.time;
                }

                if (gameObject.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
                {
                    trueAngle = (float)Math.Atan2(screenPos.y - player.transform.position.y, screenPos.x - player.transform.position.x);
                    transform.rotation = LookAtTarget(GetComponent<Rigidbody2D>().velocity);
                }
            }
            catch (Exception e)
            {
                //Just...shhhh....
            }
        }
    }

    public static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }
}
