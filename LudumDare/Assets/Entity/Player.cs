using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public static Player Instance;
    public Controlls Controll;

    private float _horizontalDir;
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

    private void OnEnable()
    {
        Controll.Human.Move.performed += ctx => GetHorizontalDir(ctx.ReadValue<float>());
        Controll.Human.Move.canceled += ctx => GetHorizontalDir(0);
    }

    protected void OnDisable()
    {
        Controll.Human.Move.performed -= ctx => GetHorizontalDir(ctx.ReadValue<float>());
        Controll.Human.Move.canceled -= _ => GetHorizontalDir(0);
    }
    

    void GetHorizontalDir(float dir)
    {
        _horizontalDir = dir;
    }
    private void ChangeState()
   {
       if(Controll.Werewolf.enabled)
           Controll.Werewolf.Disable();
       else
           Controll.Werewolf.Enable();
   }

   void Update()
    {
        Move(_horizontalDir);
    }
}
