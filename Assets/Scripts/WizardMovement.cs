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

    void Start()
    {
        var float horizontal;
        // Used for the horizontal input
        var float speed = 8f;
        // The speed (world units per second) that the player travels
        var float jumpingPower = 40f;
        // The POWER (world units per second) of dem legs
        var bool isFacingRight = true;
        // The direction of the character
        var Vector2 platformSpeed = new Vector2(0, 0);
        // Ha
        var Transform platformLastPos;
    }

    void Update() // Is called once per frame
    {
        if (transform.parent != wizard.transform)
        {
            platformSpeed = new Vector2((transform.parent.transform.position.x - platformLastPos.position.x) / (1.0f / Time.deltaTime), (transform.parent.transform.position.y - platformLastPos.position.y) / (1.0f / Time.deltaTime));
            //if (platformLastPos) platformSpeed = new Vector2((transform.parent.transform.position.x - platformLastPos.x) / 50, 0);
            platformLastPos = transform.parent;
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
                if (transform.parent != wizard)
                {
                    transform.SetParent(wizard.transform, true);
                    playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x + platformSpeed.x, jumpingPower + platformSpeed.y);
                }
                else
                {
                    playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpingPower);
                }
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
        playerRigidbody.velocity = new Vector2(horizontal * speed, playerRigidbody.velocity.y);
        // The horizontal velocity is multiplied by the speed.
        // This is separate from framerate so that, if the game changes in framerate, there's not an exponential increase of speed.
        // Fun Fact, you'll find that mistake abused in Super Mario 64 speedruns.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Moving Platform")
        {
            transform.SetParent(collision.transform, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Moving Platform")
        {
            transform.SetParent(wizard.transform, true);
        }
    }
}
