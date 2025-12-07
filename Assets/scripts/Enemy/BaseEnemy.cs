using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    //first keyword in a variable declaration indicates its access modifier
    //public - accessible from other classes
    //private - accessible only within the same class
    //protected - accessible within the same class and by derived class instances
    protected SpriteRenderer sr;
    protected Animator anim;
    protected int health;

    public int maxHealth = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (maxHealth <= 0)
        {
            Debug.LogError("Max health value should be a value greater than 0. Setting to default value of 5.");
            maxHealth = 5;
        }
        health = maxHealth;
    }

    public virtual void TakeDamage(int damageVaue, DamageType damageType = DamageType.Default)
    {
        health -= damageVaue;
        if (health <= 0)
        {
            anim.SetTrigger("Death");  
        }
    }
}

public enum DamageType
{
    Default,
    JumpedOn
}