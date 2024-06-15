using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

// This script should only be applied to the Rogue's "player" object.

public class RogueMovement : MonoBehaviour
{
    [SerializeField] private GameObject rogue;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private BoxCollider2D playerHitbox;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask[] groundLayers;
    [SerializeField] private Cooldown cooldown;
    [SerializeField] private GameObject arrowPrefab;
    // Gets other objects' components used and such

    public GameObject movingGround;
    public Vector2 movingGroundSpeed;

    public bool isMobile = true;
    public float lastMoveGroundPosX;
    public float lastMoveGroundPosY;
    public double dashSensitivity = .3;
    public float dashDistance = 5f;
    private float horizontal;
    // Used for the horizontal input
    public float speed = 12f;
    // The speed (world unit per second) that the player travels
    public float jumpingPower = 50f;
    // The POWER (world units per second) of dem legs
    private bool isFacingRight = true;
    // The direction of the character
    private float lastDirect = 0;
    private bool m_isAxisInUse = false;
    private bool dirR = false;

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

        if (isMobile)
        {
            horizontal = Input.GetAxisRaw("Horizontal");

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                if (!m_isAxisInUse)
                {
                    if (Time.time - lastDirect < dashSensitivity && (Input.GetAxisRaw("Horizontal") == 1 ? true : false) == dirR && !cooldown.IsCoolingDown)
                    {
                        // DASH
                        float playerSize = playerHitbox.size.x / 2;
                        RaycastHit2D hitWall = Physics2D.Raycast(transform.position, Vector2.right * Input.GetAxisRaw("Horizontal"));
                        Debug.DrawRay(transform.position, Vector2.right * hitWall.distance * Input.GetAxisRaw("Horizontal"), Color.red);

                        transform.position = new Vector3(rogue.transform.position.x + (((hitWall.distance > (dashDistance + playerSize)) ? (dashDistance) : (hitWall.distance - playerSize)) * Input.GetAxisRaw("Horizontal")), rogue.transform.position.y);
                        cooldown.StartCooldown();
                    }
                    else
                    {
                        lastDirect = Time.time;
                        dirR = Input.GetAxisRaw("Horizontal") == 1 ? true : false;
                    }
                    m_isAxisInUse = true;
                }
            }

            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                m_isAxisInUse = false;
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0)) {
                Instantiate(arrowPrefab, transform);
            }
        }
        else
        {
            horizontal = 0;
            lastDirect = 0;
        }
        // Gets the horizontal input from the player which is set in Unity in the [Edit -> Settings -> Input] settings
        for (int i = 0; i < groundLayers.Length; i++)
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

        if (Input.GetButtonDown("Jump") && playerRigidbody.velocity.y > 0f && isMobile)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y * 0.5f);
            // Does not change the x velocity, then multiplies the current y velocity by 50%, allowing for longer presses to get higher jumps
        }

        if ((isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) && isMobile) // Upon a change in horizontal direction,
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

        if (!movingGround) transform.parent = rogue.transform;
        Debug.Log(movingGround ? true : false);
    }

    private void FixedUpdate()  // Is called exactly 50 times per second, regardless of framerate
    {
        if (horizontal != 0 && isMobile)
        {
            playerRigidbody.velocity = new Vector2((horizontal * speed) + (horizontal > 0 ? Math.Abs(movingGroundSpeed.x) : -Math.Abs(movingGroundSpeed.x)), playerRigidbody.velocity.y);
            transform.SetParent(rogue.transform);
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
        transform.SetParent(rogue.transform);
    }
}
