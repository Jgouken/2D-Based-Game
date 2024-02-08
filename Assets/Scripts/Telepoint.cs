using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCopy;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform telepoint;
    [SerializeField] private Cooldown cooldown;
    [SerializeField] private Magivision magivision;
    // Gets other objects' components used and such

    void Start()
    {
        var playCopRender = playerCopy.AddComponent<SpriteRenderer>();
        playCopRender.sprite = player.GetComponent<SpriteRenderer>().sprite;
        playerCopy.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
        playerCopy.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        playerCopy.SetActive(false);
    }

    void Update() // Is called once per frame
    {
        if (Input.GetKey(KeyCode.LeftShift) && !cooldown.IsCoolingDown) // If Left Shift is pressed and the cooldown is over
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var playerHitbox = player.GetComponent<BoxCollider2D>();
            // Grabs the position of the mouse (relative to screen) and sets the telepoint object relative to the world
            mouseWorldPos.z = 0f;
            // The z axis doesn't quite matter in a 2D space
            telepoint.position = mouseWorldPos;
            // Sets the position of the telepoint to the world position of the mouse

            var offsetPosition = player.transform.position.y - groundCheck.position.y;
            // This is the distance between the player's position and what's considered their feet (used as the check to see if they're touching the ground).
            // This is used as the point where the player is right on top of the ground.
            RaycastHit2D hitGround = Physics2D.Raycast(telepoint.position, -Vector2.up);
            RaycastHit2D hitCieling = Physics2D.Raycast(telepoint.position, -Vector2.down);
            RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(telepoint.position.x, telepoint.position.y - hitGround.distance + offsetPosition), Vector2.left);
            RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(telepoint.position.x, telepoint.position.y - hitGround.distance + offsetPosition), Vector2.right);
            // Shoots raycasts in all directions to tell the distance between the telepoint and ground/platform
            // Note: Raycasts do not exist when starting inside of an object.
            Debug.DrawRay(telepoint.position, -Vector2.up * hitGround.distance, Color.red);
            Debug.DrawRay(telepoint.position, -Vector2.down * hitCieling.distance, Color.red);
            Debug.DrawRay(new Vector3(telepoint.position.x, (telepoint.position.y - hitGround.distance) + offsetPosition, telepoint.position.z), Vector2.left * hitLeft.distance, Color.red);
            Debug.DrawRay(new Vector3(telepoint.position.x, (telepoint.position.y - hitGround.distance) + offsetPosition, telepoint.position.z), Vector2.right * hitRight.distance, Color.red);
            // Shows the rays.

            playerCopy.transform.localScale = player.transform.localScale;

            if (hitGround.collider != null && telepoint.position.y < (telepoint.position.y - hitGround.distance) + (playerHitbox.size.y))
            // If there is ground under the telepoint and your mouse is close enough to the ground
            {
                if (Vector3.Distance(new Vector3(telepoint.position.x, telepoint.position.y - hitGround.distance), new Vector3(player.transform.position.x, player.transform.position.y + (float)0.743)) <= (magivision.visionSize / 2) - playerHitbox.size.x)
                // If the telepoint is within the magivision circle
                {
                    if (hitCieling.collider != null) // If there is cieling above the telepoint
                    {
                        if (hitGround.distance + hitCieling.distance >= playerHitbox.size.y)
                        // if the distance between the cieling and ground is big enough to fit the player
                        {
                            if (hitLeft.distance > playerHitbox.size.x && hitRight.distance > playerHitbox.size.x)
                            // If the player is not too close to nearby walls
                            {
                                if (Input.GetMouseButtonDown(0)) // Left Click
                                {
                                    player.transform.position = new Vector3(telepoint.position.x, (telepoint.position.y - hitGround.distance) + offsetPosition); // Move Player
                                    cooldown.StartCooldown(); // Start the cooldown before teleporting again
                                }
                                if (telepoint.position.y < (telepoint.position.y - hitGround.distance) + (playerHitbox.size.y * 3)) ActivateCopy(new Vector3(telepoint.position.x, (telepoint.position.y - hitGround.distance) + offsetPosition, telepoint.position.z));
                                // Shows the ghost player on the ground if there's enough vertical space
                            }
                            else playerCopy.SetActive(false);
                        }
                        else playerCopy.SetActive(false);
                    }
                    else
                    {
                        if (hitLeft.distance > playerHitbox.size.x && hitRight.distance > playerHitbox.size.x)
                        // If the player is not too close to any nearby wall
                        {
                            if (Input.GetMouseButtonDown(0)) // Left Click
                            {
                                player.transform.position = new Vector3(telepoint.position.x, (telepoint.position.y - hitGround.distance) + offsetPosition, telepoint.position.z); // Move Player
                                cooldown.StartCooldown(); // Start the coooldown before teleporting again
                            }
                            ActivateCopy(new Vector3(telepoint.position.x, (telepoint.position.y - hitGround.distance) + offsetPosition, telepoint.position.z));
                            // Shows the ghost player on the ground
                        }
                        else playerCopy.SetActive(false);
                        // Same as above
                    }
                }
                else playerCopy.SetActive(false);
            }
            else playerCopy.SetActive(false);
        }
        else playerCopy.SetActive(false);
    }

    void ActivateCopy(Vector3 newPosition)
    {
        playerCopy.SetActive(true);
        playerCopy.transform.position = newPosition;
    }
}
