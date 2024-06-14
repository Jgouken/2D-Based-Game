using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

// This script should only be applied to the Wizard's "player" object.

public class WizardMovement : MonoBehaviour
{
    [SerializeField] private GameObject wizard;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask[] groundLayers;
    // Gets other objects' components used and such

    public GameObject movingGround;
    public Vector2 movingGroundSpeed;

    public bool isMobile = true;
    public float lastMoveGroundPosX;
    public float lastMoveGroundPosY;
    private float horizontal;
    // Used for the horizontal input
    public float speed = 8f;
    // The speed (world unit per second) that the player travels
    public float jumpingPower = 40f;
    // The POWER (world units per second) of dem legs
    private bool isFacingRight = true;
    // The direction of the character
    void Update() // Is called once per frame
    {
        if (movingGround)
        {
            //if (lastGroundPos) movingGroundSpeed = new Vector2(Math.Abs(lastGroundPos.position.x - movingGround.transform.position.x), Math.Abs(lastGroundPos.position.y - movingGround.transform.position.y));
            if (lastMoveGroundPosX != 0 && lastMoveGroundPosY != 0) movingGroundSpeed = new Vector2(-(lastMoveGroundPosX - movingGround.transform.position.x) / Time.deltaTime, -(lastMoveGroundPosY - movingGround.transform.position.y) / Time.deltaTime);
            lastMoveGroundPosX = movingGround.transform.position.x;
            lastMoveGroundPosY = movingGround.transform.position.y;
        }
        else
        {
            lastMoveGroundPosX = 0;
            lastMoveGroundPosY = 0;
            movingGroundSpeed = new Vector2(0, 0);
        }

        if (isMobile) horizontal = Input.GetAxisRaw("Horizontal");
        else horizontal = 0;
        // Gets the horizontal input from the player which is set in Unity in the [Edit -> Settings -> Input] settings
        if (isMobile) for (int i = 0; i < groundLayers.Length; i++)
        {
            if (Input.GetButtonDown("Jump") && Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayers[i]) && isMobile)
            // If the "Jump" button is pressed and the groundCheck object is within anything on the groundLayer layer
            {
                // Does not change the x velocity, then sets the y velocity to the jumpingPower variable
                i = groundLayers.Length;
                //playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpingPower);
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x + movingGroundSpeed.x, jumpingPower + movingGroundSpeed.y);
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
            playerRigidbody.velocity = new Vector2((horizontal * speed) + (horizontal > 0 ? Math.Abs(movingGroundSpeed.x) : -Math.Abs(movingGroundSpeed.x)), playerRigidbody.velocity.y);
            transform.SetParent(wizard.transform);
        }
        else
        {
            if (movingGround) if (movingGround.tag == "Moving Platform") transform.SetParent(movingGround.transform);
            playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BoxCollider2D>()) if (transform.position.y >= (collision.gameObject.transform.position.y + collision.gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 2))
            {
                movingGround = collision.gameObject;
                if (movingGround.tag == "Moving Platform") transform.SetParent(movingGround.transform);
            }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        movingGround = null;
        transform.SetParent(wizard.transform);
    }
}
