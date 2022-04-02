using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CleverEntity : Entity
{
    [SerializeField] protected Transform target;

    [SerializeField] protected float lookDistance;
    [SerializeField] protected float distanceAtk;
    [SerializeField] protected LayerMask ignoreLayer;
    protected bool seeTarget;
   protected override void Start()
   {
        base.Start();
        target = Player.Instance.transform;

   }
  
    protected bool CanSeeTarget()
    {
        var hit = Physics2D.Raycast(transform.position,  target.transform.position - transform.position , lookDistance, ~ignoreLayer );
        bool see = hit.collider?.transform == target;
        Debug.DrawRay(transform.position, target.transform.position - transform.position, see?Color.green:Color.red);
        return see;
    }

    protected bool CanAtk()
    {
        return seeTarget && Vector2.Distance(target.transform.position, transform.position) <= distanceAtk;
    }
    
    void Update()
    {
        seeTarget = CanSeeTarget();
        
    }
}
