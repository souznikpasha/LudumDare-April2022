using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    [SerializeField] protected int hp;
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpForce;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        
    }
    protected virtual void Move(float x)
    {
        rb.velocity = new Vector2(x * speed, rb.velocity.y);

    }

    protected virtual void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    
    public void TakeDamage(int damage)
    {
        if (hp > 0)
        {
            hp -= damage;
            if(hp<=0) 
                Die();
        }
    }

    void Die()
    {
        Destroy(this);
    }
}
