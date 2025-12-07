using UnityEngine;

public class TurretEnemy : BaseEnemy
{
    [SerializeField] private float distThreshold = 6.0f; //distance to detect player
    [SerializeField] private float fireRate = 2.0f; //seconds between shots
    private float timeSinceLastFire = 0;

    private controller playerRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        if (fireRate <= 0)
        {
            Debug.LogError("Fire rate must be greater than zero, set to default value of 2");
            fireRate = 2.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerRef) return;

        if (!CheckDistance())
        {
            sr.color = Color.white;
            return;
        }

        //face player and get aggroed
        sr.flipX = (transform.position.x > playerRef.transform.position.x);
        sr.color = Color.red;

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Idle"))
        {
            //trigger fire animation logic
            if (Time.time >= timeSinceLastFire + fireRate)
            {
                anim.SetTrigger("Fire");
                timeSinceLastFire = Time.time;
            }
        }
    }

    bool CheckDistance()
    {
        float distToPlayer = Vector3.Distance(transform.position, playerRef.transform.position);
        Debug.Log("Distance to player: " + distToPlayer);
        return distToPlayer <= distThreshold;
    }    
}
