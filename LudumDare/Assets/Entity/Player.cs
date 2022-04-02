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
    [Range(0,5)]
    [SerializeField] private float jumpDamping;
    public PlayerState State { get; private set; }
    [Header(header: "Player Conditions")]
    [SerializeField] private float _humanSpeed;
    [SerializeField] private float _werewolfSpeed;
    [SerializeField] private float _humanJumpForce;
    [SerializeField] private float _werewolfJumpForce;
    private float _horizontalDir;
    private CapsuleCollider2D _collider;
    private float _activeJumpForce;
    private bool _onGround;
    private int _werewolfPercent = 0;
    private bool _underMoon;
    private bool _moonAccumulationActive;
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
        ChangeState(PlayerState.Human);
        
    }

    private void OnEnable()
    {
        Controll.Human.Move.performed += ctx => GetHorizontalDir(ctx.ReadValue<float>());
        Controll.Human.Move.canceled += ctx => GetHorizontalDir(0);
        Controll.Human.Jump.performed +=_ => StartJump();
        Controll.Human.Jump.canceled += _ => _activeJumpForce = 0;
        Controll.Human.Roar.performed += _ => Roar();
    }
    
    protected void OnDisable()
    {
        Controll.Human.Move.performed -= ctx => GetHorizontalDir(ctx.ReadValue<float>());
        Controll.Human.Move.canceled -= _ => GetHorizontalDir(0);
        Controll.Human.Jump.performed -=_ => StartJump();
        Controll.Human.Jump.canceled -= _ => _activeJumpForce = 0;
        Controll.Human.Roar.performed -= _ => Roar();
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
        _horizontalDir = dir;
    }
    private void ChangeState(PlayerState state)
   {
       State = state;
       if (state == PlayerState.Human)
       {
           jumpForce = _humanJumpForce;
           speed = _humanSpeed;
           Controll.Werewolf.Disable();
       }
       else
       {
           jumpForce = _werewolfJumpForce;
           speed = _werewolfSpeed;
           Controll.Werewolf.Enable();
       }
   }

    IEnumerator MoonAccumulations()
    {
        if (!_moonAccumulationActive)
        {
            _moonAccumulationActive = true;
            while (_underMoon)
            {
                _werewolfPercent++;
                yield return new WaitForSeconds(1);
            }

            _moonAccumulationActive = false;
        }
    }
    public void SetMoonState(bool underMoon)
    {
        _underMoon = underMoon;
        if (_underMoon)
        {
           
            StartCoroutine(MoonAccumulations());
        }
    }

    public void Roar()
    {
        switch (State)
        {
            case PlayerState.Human:
                Debug.Log("Player Roar");
                break;
            case PlayerState.WereWolf:
                Debug.Log("Werewolf Roar");
                break;
        }
    }
    protected override void Move(float x)
    {
        if(_onGround)
            rb.velocity = new Vector2(x * speed, rb.velocity.y);
        else
        {
            rb.velocity = new Vector2(rb.velocity.x + x*speed*Time.deltaTime, rb.velocity.y);
        }
        
    }

    void Update()
   {
      _onGround = OnGround(_collider.size.y / 2);
       Move(_horizontalDir);
        Jump();
   }
}
