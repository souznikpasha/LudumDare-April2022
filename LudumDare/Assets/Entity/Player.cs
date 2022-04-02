using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public static Player Instance;
    public Controlls Controll;
    public PlayerState State { get; private set; }
    [SerializeField] private float jumpDamping;
    
    private float _horizontalDir;
    private CapsuleCollider2D _collider;
    private float _activeJumpForce;
    private bool _onGround;
    private int _werewolfPercent = 0;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
        }

        Controll = new Controlls();
        Controll.Human.Enable();
        Controll.Werewolf.Disable();
        
    }

    protected override void Start()
    {
        base.Start();
        _collider = GetComponent<CapsuleCollider2D>();
        State = PlayerState.Human;
    }

    private void OnEnable()
    {
        Controll.Human.Move.performed += ctx => GetHorizontalDir(ctx.ReadValue<float>());
        Controll.Human.Move.canceled += ctx => GetHorizontalDir(0);
        Controll.Human.Jump.performed +=_ => StartJump();
        Controll.Human.Jump.canceled += _ => _activeJumpForce = 0;
    }
    
    protected void OnDisable()
    {
        Controll.Human.Move.performed -= ctx => GetHorizontalDir(ctx.ReadValue<float>());
        Controll.Human.Move.canceled -= _ => GetHorizontalDir(0);
        Controll.Human.Jump.performed -=_ => StartJump();
        Controll.Human.Jump.canceled -= _ => _activeJumpForce = 0;
    }

    void StartJump()
    {
        if (_onGround)
        {
            _activeJumpForce = jumpForce;
        }
    }
    void JumpDamping()
    {
        if (_activeJumpForce > 0)
            _activeJumpForce -= jumpDamping;
        if (_activeJumpForce < 0)
            _activeJumpForce = 0;
    }

    protected override void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, _activeJumpForce + rb.velocity.y);
        JumpDamping();
    }

    void GetHorizontalDir(float dir)
    {
        if(_onGround)
            _horizontalDir = dir;
    }
    private void ChangeState()
   {
       if(Controll.Werewolf.enabled)
           Controll.Werewolf.Disable();
       else
           Controll.Werewolf.Enable();
   }

    public void SetMoon()
    {
        _werewolfPercent++;
    }
   void Update()
   {
      _onGround = OnGround(_collider.size.y / 2);
       Move(_horizontalDir);
        Jump();
   }
}
