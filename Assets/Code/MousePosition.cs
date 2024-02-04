using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private BoxCollider2D playerHitbox;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform telepoint;
    // Gets other objects' components used and such

    void Update() // Is called once per frame
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Grabs the position of the mouse (relative to screen) and sets the telepoint object relative to the world
        mouseWorldPos.z = 0f;
        // The z axis doesn't quite matter in a 2D space
        telepoint.position = mouseWorldPos;
        // Sets the position of the telepoint to the world position of the mouse

        var offsetPosition = playerPosition.position.y - groundCheck.position.y;
        // This is the distance between the player's position and what's considered their feet (used as the check to see if they're touching the ground).
        // This is used as the point where the player is right on top of the ground.
        RaycastHit2D hitGround = Physics2D.Raycast(telepoint.position, -Vector2.up);
        RaycastHit2D hitCieling = Physics2D.Raycast(telepoint.position, -Vector2.down);
        // Shoots raycasts up and down to tell the distance between the telepoint and ground/platform
        // Note: Raycasts do not exist when starting inside of an object.
        Debug.DrawRay(telepoint.transform.position, -Vector2.up * hitGround.distance, Color.red);
        Debug.DrawRay(telepoint.transform.position, -Vector2.down * hitCieling.distance, Color.red);
        // Shows the "Scene" window the rays.

        if (hitGround.collider != null) // If there is ground under the telepoint
        {
            if (hitCieling.collider != null) // If there is cieling above the telepoint
            {
                if (hitGround.distance + hitCieling.distance >= playerHitbox.size.y)
                // if the distance between the cieling and ground is big enough to fit the player
                {
                    if (Input.GetMouseButtonDown(0)) // Left Click
                    {
                        playerPosition.position = new Vector3(telepoint.position.x, (telepoint.position.y - hitGround.distance) + offsetPosition, telepoint.position.z);
                        /**
                            Moves the player to:
                            X: The same X as the telepoint
                            Y: The offsetPosition distance to stand on the ground
                            Z: Doesn't matter*, but set to the same as the telepoint

                            * It does matter actually, but its useless for now.
                        */
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    playerPosition.position = new Vector3(telepoint.position.x, (telepoint.position.y - hitGround.distance) + offsetPosition, telepoint.position.z);
                    // Same as above
                }
            }
        }
    }
}
