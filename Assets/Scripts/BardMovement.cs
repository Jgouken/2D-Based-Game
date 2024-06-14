using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

// This script should only be applied to the bard's "player" object.

public class BardMovement : MonoBehaviour
{
    [SerializeField] private GameObject bard;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask[] groundLayers;
    [SerializeField] private Cooldown cooldown;

    // Arrows
    public GameObject left;
    public GameObject right;
    public GameObject down;
    public GameObject up;
    private GameObject currentArrow;

    public List<GameObject> arrowObjects = new List<GameObject>();

    // Gets other objects' components used and such
    public bool isMobile = true;
    public GameObject movingGround;
    public Vector2 movingGroundSpeed;
    public string arrowCode = "";
    public string submittedCode = "";
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

        float lbut = Input.GetKey(KeyCode.A) ? -1 : 0;
        float rbut = Input.GetKey(KeyCode.D) ? 1 : 0;
        horizontal = lbut + rbut;
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

        if (Input.GetKey(KeyCode.LeftShift) && !cooldown.IsCoolingDown && isMobile)
        {
            if (arrowCode.Length < 10)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    // Left
                    arrowCode = arrowCode + "1";
                    currentArrow = Instantiate(left, transform);
                    currentArrow.transform.position = new Vector3(currentArrow.transform.position.x, currentArrow.transform.position.y + 2);
                    currentArrow.transform.localScale = new Vector3(-1, 1, 0);
                    foreach (var arrow in arrowObjects)
                    {
                        // GET OUT OF THE WAY, DAYUM
                        arrow.transform.position = new Vector3(arrow.transform.position.x - (isFacingRight == true ? 2 : -2), arrow.transform.position.y);
                    }
                    arrowObjects.Add(currentArrow);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    // Right
                    arrowCode = arrowCode + "4";
                    currentArrow = Instantiate(right, transform);
                    currentArrow.transform.position = new Vector3(currentArrow.transform.position.x, currentArrow.transform.position.y + 2);
                    currentArrow.transform.localScale = new Vector3(-1, 1, 0);
                    foreach (var arrow in arrowObjects)
                    {
                        // GET OUT OF THE WAY, DAYUM
                        arrow.transform.position = new Vector3(arrow.transform.position.x - (isFacingRight == true ? 2 : -2), arrow.transform.position.y);
                    }
                    arrowObjects.Add(currentArrow);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    // Up
                    arrowCode = arrowCode + "2";
                    currentArrow = Instantiate(up, transform);
                    currentArrow.transform.position = new Vector3(currentArrow.transform.position.x, currentArrow.transform.position.y + 2);
                    foreach (var arrow in arrowObjects)
                    {
                        // GET OUT OF THE WAY, DAYUM
                        arrow.transform.position = new Vector3(arrow.transform.position.x - (isFacingRight == true ? 2 : -2), arrow.transform.position.y);
                    }
                    arrowObjects.Add(currentArrow);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    // Down
                    arrowCode = arrowCode + "3";
                    currentArrow = Instantiate(down, transform);
                    currentArrow.transform.position = new Vector3(currentArrow.transform.position.x, currentArrow.transform.position.y + 2);
                    foreach (var arrow in arrowObjects)
                    {
                        // GET OUT OF THE WAY, DAYUM
                        arrow.transform.position = new Vector3(arrow.transform.position.x - (isFacingRight == true ? 2 : -2), arrow.transform.position.y);
                    }
                    arrowObjects.Add(currentArrow);
                }
            }
        }
        else
        {
            if (arrowCode != "" && isMobile)
            { // Such a weird way of doing it, I hate it
                submittedCode = arrowCode;
                switch (arrowCode)
                {
                    case "231":
                        {
                            Debug.Log("Rain");
                            cooldown.StartCooldown();
                            break;
                        }
                    case "234":
                        {
                            Debug.Log("Sun");
                            cooldown.StartCooldown();
                            break;
                        }
                    case "231432":
                        {
                            Debug.Log("Time");
                            cooldown.StartCooldown();
                            break;
                        }
                    case "413":
                        {
                            Debug.Log("Entrance");
                            cooldown.StartCooldown();
                            break;
                        }
                }
                arrowCode = "";
                foreach (var arrow in arrowObjects)
                {
                    Destroy(arrow);
                }
                arrowObjects = new List<GameObject>();
            }
        }
    }

    private void FixedUpdate()  // Is called exactly 50 times per second, regardless of framerate
    {
        if (horizontal != 0 && isMobile)
        {
            playerRigidbody.velocity = new Vector2((horizontal * speed) + (horizontal > 0 ? Math.Abs(movingGroundSpeed.x) : -Math.Abs(movingGroundSpeed.x)), playerRigidbody.velocity.y);
            transform.SetParent(bard.transform);
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
        transform.SetParent(bard.transform);
    }
}
