using UnityEngine;

public class GlorbMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D enemyRigidbody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask[] groundLayers;
    public GameObject movingGround;
    public Vector2 movingGroundSpeed;

    void Update()
    {
        for (int i = 0; i < groundLayers.Length; i++)
        {
            if (!Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayers[i]))
            {
                // Me when I can't think...I'll come back to this later.
            }
        }
    }
}
