using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float horizontal;
    // Used for the horizontal input
    public float speed = 8f;
    // The speed (world units per second) that the player travels
    public float jumpingPower = 31f;
    // The POWER (world units per second) of dem legs
    private bool isFacingRight = false;
    // The direction of the character
    public char playerClass = 'W';
    // The type of character (currently unused)

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform mousePosition;
    [SerializeField] private LayerMask groundLayer;
    // Gets other objects' components used and such
    void Update() // Is called once per frame
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        // Gets the horizontal input from the player which is set in Unity in the [Edit -> Settings -> Input] settings
        if (Input.GetButtonDown("Jump") && Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        // If the "Jump" button is pressed and the groundCheck object is within anything on the groundLayer layer
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            // Does not change the x velocity, then sets the y velocity to the jumpingPower variable
        }

        if (Input.GetButtonDown("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
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

        switch (playerClass)
        {
            case 'W':
                {
                    // I AM A WIZARD
                    break;
                }
            case 'R':
                {
                    // I AM A ROGUE
                    break;
                }
            case 'B':
                {
                    // I AM A BARD
                    break;
                }
            default:
                {
                    // I AM AN ERROR
                    break;
                }
        }
    }

    private void FixedUpdate()  // Is called exactly 50 times per second, regardless of framerate
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        // The horizontal velocity is multiplied by the speed.
        // This is separate from framerate so that, if the game changes in framerate, there's not an exponential increase of speed.
        // Fun Fact, you'll find that mistake abused in Super Mario 64 speedruns.
    }
}
