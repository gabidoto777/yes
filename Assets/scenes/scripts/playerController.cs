using UnityEngine;

public class controller : MonoBehaviour
{

    // Component References
    //public Transform groundCheck;
    Rigidbody2D rb;
    // References to the Collider2D component
    Collider2D col;
    // Reference to the SpriteRenderer component
    SpriteRenderer sr;
    // Reference to the Animator component
    Animator anim;
    // LayerMask to identify ground objects
    private LayerMask groundLayer;


    //Control variables
    // Movement speed of the player
    public float moveSpeed = 10f;
    // Radius for ground check
    public float groundCheckRadius = 0.02f;
    // Is the player currently grounded
    private bool isGrounded = false;

    // Is the player currently falling
    public bool isFalling = false;


    // Calculate ground check position based on collider bounds
    private Vector2 groundCheckPos => new Vector2(col.bounds.center.x, col.bounds.min.y);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Get the Rigidbody2D component attached to the player
        rb  = GetComponent<Rigidbody2D>();
        // Get the Collider2D component attached to the player
        col = GetComponent<Collider2D>();
        // Get the SpriteRenderer component attached to the player
        sr = GetComponent<SpriteRenderer>();
        // Get the Animator component attached to the player
        anim = GetComponent<Animator>();
        // Initialize the ground layer mask
        groundLayer = LayerMask.GetMask("Ground");

        //other option to
        //initialize ground check position using separate GameObject as a child of the player
        //GameObject newObj = new GameObject("GroundCheck");
        //newObj.transform.SetParent(transform);
        //newObj.transform.localPosition = Vector3.zero;
        // groundCheck = newObj.transform;
        //this is basically the same as creating an empty GameObject in the Unity Editor and assigning it to groundCheck, we do it here to keep everything contained in code.

    }

    // Update is called once per frame
    void Update()
    {

        // Get horizontal input
        float hValue = Input.GetAxis("Horizontal");


        // Get vertical input
        float vValue = Input.GetAxis("Vertical");


        // Set the horizontal velocity based on input
        rb.linearVelocityX = hValue * moveSpeed;


        // Set the vertical velocity based on input
        rb.linearVelocityY = vValue * moveSpeed;
       


        // Check if the player is grounded using OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer);
        // Flip the sprite based on movement direction
        if (hValue != 0)
            sr.flipX = hValue < 0;
        // Jump when the jump button is pressed
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
        // Update animator parameters
        anim.SetFloat("hValue",Mathf.Abs(hValue));
        anim.SetBool("isGrounded", isGrounded);

    }
}
