using System.ComponentModel;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent (typeof(Animator))]
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
    // Reference to the GroundCheck script
    GroundCheck groundCheckScript;


    // LayerMask to identify ground objects
    // LayerMask groundLayer;



    //Control variables
    // Movement speed of the player
    public float moveSpeed = 10f;
    // Radius for ground check
    public float groundCheckRadius = 0.02f;

    public bool isGrounded = false;

    private bool isFalling = false;
    public bool IsFalling => isFalling;

    public bool isCrouching = false;
    public bool isParachuting = false;

    private float decelRate = 0;


    // Calculate ground check position based on collider bounds
    //private Vector2 groundCheckPos => new Vector2(col.bounds.center.x, col.bounds.min.y);


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

         
        groundCheckScript = new GroundCheck(col, LayerMask.GetMask("Ground"), groundCheckRadius);

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

        isGrounded = groundCheckScript.CheckisGrounded();

        isCrouching = Input.GetButton("Fire3") && isGrounded;

        if (Input.GetButton("Fire3") && isCrouching)
        {
            decelRate += Time.deltaTime;
            Mathf.Clamp(decelRate, 0f, 1f);
            moveSpeed = Mathf.Lerp(moveSpeed, 0f, decelRate);
        }
        else
        {
            //reset moveSpeed and decelRate when not crouching
            moveSpeed = 10f;
            decelRate = 0f;
        }
        // Update isFalling status
        isFalling = rb.linearVelocityY < 0;
        if (isFalling == true)
        {
            rb.gravityScale = 3f; //increase gravity when falling
        }
        else
        {
            rb.gravityScale = 1f; //reset gravity when not falling
        }

        isParachuting = Input.GetButton("Jump") && isFalling;
        if (isParachuting == true && Input.GetButton("Jump"))
        {
            rb.gravityScale = 0.2f; //reduce gravity when parachuting
        }

        if(Input.GetButton("Vertical") && isParachuting)
        {
            rb.gravityScale = 3f;
        }

        if(Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Fire");
        }

        // Get horizontal input
        float hValue = Input.GetAxis("Horizontal");

        float vValue = Input.GetAxis("Vertical");

        // Smoothly update the player's horizontal velocity
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, new Vector2(hValue * moveSpeed, rb.linearVelocity.y), 0.1f);
        // Check if the player is grounded using OverlapCircle
        //isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer);
        // Flip the sprite based on movement direction
        if (hValue != 0)
            sr.flipX = hValue < 0;
        // Jump when the jump button is pressed
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocityY -= 1.5f; //simulate weight increase when jumping
            rb.AddForce(Vector2.up * 12f, ForceMode2D.Impulse);
        }
        // Update animator parameters
        anim.SetFloat("hValue",Mathf.Abs(hValue));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isFalling", isFalling);
        anim.SetBool("isCrouching", isCrouching);
        anim.SetBool("isParachuting", isParachuting);
        anim.SetFloat("vValue", Mathf.Abs(vValue));
    }
}
