using System.ComponentModel;
using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent (typeof(Animator))]
public class controller : MonoBehaviour
{

    // COMPONENT REFERENCES
    Rigidbody2D rb;
    // REFERENCE TO THE COLLIDER2D COMPONENT
    Collider2D col;
    // REFERENCE TO THE SPRITERENDERER COMPONENT
    SpriteRenderer sr;
    // REFERENCE TO THE ANIMATOR COMPONENT
    Animator anim;
    // REFERENCE TO THE GROUNDCHECK SCRIPT
    GroundCheck groundCheckScript;

    // CONTROL VARIABLES
    //MOVE SPEED
    public float moveSpeed = 10f;
    // RADIUS FOR GROUND CHECK
    public float groundCheckRadius = 0.02f;
    // LAYER MASK FOR GROUND DETECTION
    private bool isGrounded = false;
    private bool isFalling = false;
    private bool IsFalling => isFalling;
    private bool isCrouching = false;
    private bool  isParachuting = false;
    private float decelRate = 0;
    public float jumpForce = 10.5f;


    // STATE COROUTINES
    Coroutine jumpForceCoroutine;


    // START IS CALLED BEFORE THE FIRST FRAME UPDATE
    void Start()
    {

        // GET THE RIGIDBODY2D COMPONENT
        rb = GetComponent<Rigidbody2D>();
        // Get THE COLLIDER2D COMPONENT
        col = GetComponent<Collider2D>();
        // GET THE SPRITERENDERER COMPONENT
        sr = GetComponent<SpriteRenderer>();
        // GET THE ANIMATOR COMPONENT
        anim = GetComponent<Animator>();
        // INITIALIZE THE GROUNDCHECK SCRIPT
        groundCheckScript = new GroundCheck(col, LayerMask.GetMask("Ground"), groundCheckRadius);



        /*ANOTHER OPTION IS TO INITIALIZE THE GROUND CHECK POSITION USING A SEPARATE GAMEOBJECT AS A CHILD OF THE PLAYER
        *
        *
        *
        GameObject newObj = new GameObject("GroundCheck");
        newObj.transform.SetParent(transform);
        newObj.transform.localPosition = Vector3.zero;
        groundCheck = newObj.transform;
        *
        *
        *
        THIS IS ESSENTIALLY THE SAME AS CREATING AN EMPTY GAMEOBJECT IN THE UNITY EDITOR AND ASSIGNING IT TO GROUNDCHECK — WE'RE JUST DOING IT HERE IN CODE TO KEEP EVERYTHING SELF-CONTAINED*/

    }

    // UPDATE IS CALLED ONCE PER FRAME
    void Update()
    {
        // UPDATE isGrounded STATUS
        isGrounded = groundCheckScript.CheckisGrounded();


        // UPDATE isCrouching STATUS
        isCrouching = Input.GetButton("Fire3") && isGrounded;


        // DECELERATE MOVEMENT WHEN CROUCHING
        if (Input.GetButton("Fire3") && isCrouching)
        {
            decelRate += Time.deltaTime;
            Mathf.Clamp(decelRate, 0f, 1f);
            moveSpeed = Mathf.Lerp(moveSpeed, 0f, decelRate);
        }
        else
        {
            // RESET MOVE SPEED AND DECEL RATE WHEN NOT CROUCHING
            moveSpeed = 10f;
            decelRate = 0f;
        }


        // UPDATE isFalling STATUS
        isFalling = rb.linearVelocityY < 0;
        if (isFalling == true)
        {
            rb.gravityScale = 3f; // INCREASE GRAVITY WHEN FALLING
        }
        else
        {
            rb.gravityScale = 1f; // RESET GRAVITY WHEN NOT FALLING
        }



        // UPDATE isParachuting STATUS
        isParachuting = Input.GetButton("Jump") && isFalling;
        if (isParachuting == true && Input.GetButton("Jump"))
        {
            rb.gravityScale = 0.2f; // REDUCE GRAVITY WHEN PARACHUTING
        }


        // INCREASE GRAVITY WHEN MOVING VERTICALLY WHILE PARACHUTING
        if (Input.GetButton("Vertical") && isParachuting)
        {
            rb.gravityScale = 3f;
        }


        // FIRE ACTION
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Fire");
        }


        // GETTING INPUT VALUES FOR HORIZONTAL AND VERTICAL MOVEMENT
        float hValue = Input.GetAxis("Horizontal");
        float vValue = Input.GetAxis("Vertical");


        // SMOOTH PLAYER MOVEMENT
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, new Vector2(hValue * moveSpeed, rb.linearVelocity.y), 0.1f);


        /* CHECKING IF THE PLAYER IS GROUNDED USING OVERLAPCIRCLE (METHOD REPLACED BY GROUNDCHECK SCRIPT)
        isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer); */


        // FLIPPING THE SPRITE BASED ON MOVEMENT DIRECTION
        if (hValue != 0)
            sr.flipX = hValue < 0;


        // JUMPING
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocityY -= 1.5f; //simulate weight increase when jumping
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }



        // ANIMATOR PARAMETERS UPDATE
        anim.SetFloat("hValue",Mathf.Abs(hValue));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isFalling", isFalling);
        anim.SetBool("isCrouching", isCrouching);
        anim.SetBool("isParachuting", isParachuting);
        anim.SetFloat("vValue", Mathf.Abs(vValue));
    }


    // Collision and Trigger Event Handlers
    private void OnCollissionEnter2D(Collision2D collision)
    {

    }

    private void OnCollissionStay2D(Collision2D collision)
    {
        
    }

    private void OnCollissionExit2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
    }


    // Trigger event handlers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void ApplyJumpForcePowerup()
    {
        if (jumpForceCoroutine != null)
        {
            StopCoroutine(jumpForceCoroutine);
            jumpForceCoroutine = null;
            jumpForce = 10.5f; // RESET JUMP FORCE TO DEFAULT
        }

        jumpForceCoroutine = StartCoroutine(JumpForceChange());
    }

    // JUMP FORCE POWERUP COROUTINE (USING SYSTEM.COLLECTIONS)
    IEnumerator JumpForceChange()
    {
        jumpForce = 12;
        yield return new WaitForSeconds(5f);
        jumpForce = 10.5f;
        jumpForceCoroutine = null;
    }
}
