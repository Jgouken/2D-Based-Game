using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should only be applied to the Wizard's "player" object.

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject wizard;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask[] groundLayers;
    // Gets other objects' components used and such

    public GameObject currentGround;
    public Vector2 groundSpeed;

    public float lastGroundPosX;
    public float lastGroundPosY;
    private float horizontal;
    // Used for the horizontal input
    private float speed = 8f;
    // The speed (world unit per second) that the player travels
    private float jumpingPower = 40f;
    // The POWER (world units per second) of dem legs
    private bool isFacingRight = true;
    // The direction of the character

    void Update() // Is called once per frame
    {
        if (currentGround)
        {
            //if (lastGroundPos) groundSpeed = new Vector2(Math.Abs(lastGroundPos.position.x - currentGround.transform.position.x), Math.Abs(lastGroundPos.position.y - currentGround.transform.position.y));
            if (lastGroundPosX != 0 && lastGroundPosY != 0) groundSpeed = new Vector2(-(lastGroundPosX - currentGround.transform.position.x) / Time.deltaTime, -(lastGroundPosY - currentGround.transform.position.y) / Time.deltaTime);
            lastGroundPosX = currentGround.transform.position.x;
            lastGroundPosY = currentGround.transform.position.y;
        }
        else
        {
            lastGroundPosX = 0;
            lastGroundPosY = 0;
            groundSpeed = new Vector2(0, 0);
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        // Gets the horizontal input from the player which is set in Unity in the [Edit -> Settings -> Input] settings
        for (int i = 0; i < groundLayers.Length; i++)
        {
            if (Input.GetButtonDown("Jump") && Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayers[i]))
            // If the "Jump" button is pressed and the groundCheck object is within anything on the groundLayer layer
            {
                // Does not change the x velocity, then sets the y velocity to the jumpingPower variable
                i = groundLayers.Length;
                //playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpingPower);
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x + groundSpeed.x, jumpingPower + groundSpeed.y);
            }
        }

        if (Input.GetButtonDown("Jump") && playerRigidbody.velocity.y > 0f)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y * 0.5f);
            // Does not change the x velocity, then multiplies the current y velocity by 50%, allowing for longer presses to get higher jumps
        }

        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) // Upon a change in horizontal direction,
        {
            isFacingRight = !isFacingRight;
            // Sets the facing direction to the other way
            Vector3 localScale = transform.localScale;
            // Grabs the scale of the player
            localScale.x *= -1f;
            // Flips the horizontal (x) scale of the player
            transform.localScale = localScale;
            // Applies it to the player
        }
    }

    private void FixedUpdate()  // Is called exactly 50 times per second, regardless of framerate
    {
        if (horizontal != 0)
        {
            playerRigidbody.velocity = new Vector2(horizontal * speed, playerRigidbody.velocity.y);
            transform.SetParent(wizard.transform);
        }
        else
        {
            if (currentGround)
            {
                playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
                if (currentGround.tag == "Moving Platform") transform.SetParent(currentGround.transform);
            } else playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BoxCollider2D>()) if (transform.position.y >= (collision.gameObject.transform.position.y + collision.gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 2))
            {
                currentGround = collision.gameObject;
                if (currentGround.tag == "Moving Platform") transform.SetParent(currentGround.transform);
            }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        currentGround = null;
        transform.SetParent(wizard.transform);
    }
}
