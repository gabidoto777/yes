using UnityEngine;

public class GroundCheck
{
    private bool isGrounded;
    public bool IsGrounded => isGrounded;

    private LayerMask groundLayer;
    private Collider2D col;
    private Rigidbody2D rb;
    private float groundCheckRadius = 0.02f;
    private Vector2 groundCheckPos => new Vector2(col.bounds.center.x, col.bounds.min.y);

    public GroundCheck(Collider2D col, LayerMask groundLayer,float groundCheckRadius)
    {
    
        this.col = col;
        this.groundLayer = groundLayer;
        this.groundCheckRadius = groundCheckRadius;
        rb = col.GetComponent<Rigidbody2D>();

    }

    public bool CheckisGrounded()
    {
        if (!isGrounded && rb.linearVelocityY < 0 || isGrounded)
            isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer);

        return isGrounded;
    }
}
 