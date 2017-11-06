using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    Camera MainCamera;
    Animator Ani;    
    Transform StoneKing;
    BladeWork Blade;

    ePlayerState CurrentState = ePlayerState.AttackReady;
    ePlayerState NextState = ePlayerState.none;
    public float Speed = 0;
    float JumpPower = 5.5f;
    float AttackTime = 0;
    bool IsMove = false;
    bool IsJump = false;    
    bool IsAttack = false;    
    int AttackCnt = 0;

    public bool IsBladeWork { get; set; }
    
    protected override void Awake()
    {
        IsPlayer = true;
        base.Awake();        
    }

    void Start ()
    {
        MainCamera = Camera.main;
        Ani = GetComponent<Animator>();
        StoneKing = this.transform.Find("Bip01");
        Blade = GetComponent<BladeWork>();
    }
    
    protected override void Update()
    {
        KeyInput();
        Attack();
        ChangeAnimation();
    }

    private void FixedUpdate()
    {        
        Move();        
    }

    void KeyInput()
    {
        if (Input.GetButtonDown("Jump"))
        {            
            Jump();            
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) && IsBladeWork == false)
        {
            Blade.Initialize();
            IsBladeWork = true;
        }
    }


    void Move()
    {
        if(IsAttack)
        {
            int index = Ani.GetInteger("State");

            if(index >= (int)ePlayerState.Attack1 && index <= (int)ePlayerState.Attack4)
            {
                if (!Ani.IsInTransition(0))
                    return;

                return;
            }            
        }
        
        Vector3 MoveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                
        if (MoveInput != Vector3.zero)
        {
            IsMove = true;            
            transform.forward = MoveInput.normalized;            

            if (!IsJump)
            {
                NextState = ePlayerState.Run;                
            }
        }
        else
        {
            IsMove = false;

            if(!IsJump && !IsAttack)
                NextState = ePlayerState.AttackReady;            
        }

        transform.Translate(MoveInput.normalized * Speed * Time.deltaTime, Space.World);
        
    }

    void Jump()
    {
        if (IsJump)
            return; 

        NextState = ePlayerState.Jump;
        IsJump = true;

        Rigidbody rigid = GetComponent<Rigidbody>();

        rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);     
    }

    void Attack()
    {
        
        if(Input.GetKeyDown(KeyCode.Z))
        {
            IsAttack = true;

            if (IsJump)
                NextState = ePlayerState.Attack4;
            else
            {
                NextState = ePlayerState.Attack3_1 + AttackCnt;

                if((int)NextState != Ani.GetInteger("State"))
                    ++AttackCnt;

                if (AttackCnt >= 3)
                    AttackCnt = 0;

                AttackTime = 0;
            }
            
        }

        if (IsAttack)
        {
            AttackTime += Time.deltaTime;

            if (AttackTime >= 1)
            {
                AttackTime = 0;
                AttackCnt = 0;
                IsAttack = false;
            }
        }
    }

    bool IsOneTimePlay()
    {
        return false;
    }

    void ChangeAnimation()
    {
        if (NextState == ePlayerState.none)
            return;

        CurrentState = NextState;

        Ani.SetInteger("State", (int)CurrentState);
        
        NextState = ePlayerState.none;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (IsJump)
            IsJump = false;
    }









}
