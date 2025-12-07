using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkerEnemy : BaseEnemy
{
    public float xVelocity = 2;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Walk"))
        {
            //equavalent to the if else statement below - this is using the ternary operator (?)
            rb.linearVelocityX = (sr.flipX) ? xVelocity : -xVelocity;

            //if (sr.flipX) rb.linearVelocityX = -xVelocity;
            //else rb.linearVelocityX = xVelocity;
        }
    }

    public override void TakeDamage(int damageValue, DamageType damageType = DamageType.Default)
    {
        if (damageType == DamageType.JumpedOn)
        {
            anim.SetTrigger("Squish");
            Destroy(transform.parent.gameObject, 0.5f);
            return;
        }

        base.TakeDamage(damageValue, damageType);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier"))
        {
            sr.flipX = !sr.flipX;
        }
    }
}
