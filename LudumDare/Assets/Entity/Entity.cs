using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    [SerializeField] protected int hp;
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpForce;
    [SerializeField] protected LayerMask groundLayer;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    protected virtual void Move(float x)
    {
        rb.velocity = new Vector2(x * speed, rb.velocity.y);

    }

    protected bool OnGround(float halfSize)
    {
        Vector2 originVec = new Vector2(transform.position.x, transform.position.y - halfSize);
        var hit = Physics2D.Raycast(originVec, Vector2.down, 0.1f, groundLayer);
        bool on = hit.collider != null;
        Debug.DrawLine(originVec, new Vector2(originVec.x, originVec.y - 0.1f), on?Color.green:Color.blue);
        return on;
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
