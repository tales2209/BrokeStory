﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossDragonAI : BaseAI
{
    
    //공격대상
    BaseObject AttackTarget = null;

    //파이어볼 프리팹
    GameObject Prefab_Fireball = null;
    //파이어링 프리팹
    GameObject Prefab_FireRing = null;
    //BASEAI를 돌릴것인지에 대한 BOOL
    bool b_UpdateBaseAI = true;

    float time = 0; //스킬 시간체크를 위해 흐른 시간을 저장할 변수
    public float attackRange = 2f;  //일반공격 사거리
    //SKILL_1 필요한 변수
    public float Skill_Fireball_CoolTime = 10f; //파이어볼 스킬 쿨타임
    int EffectInitCount = 0;    //이펙트 생성을 제한하기위한 변수
    public float FireballInitDistance = 3f;//파이어볼 생성 거리
    bool b_Skill_1_ready = false;   //쿨타임이 끝나서 스킬 사용이 가능한지에 대한 BOOL값
    List<Skill_Fireball> List_Fireball; //생성한 파이어볼 담는 리스트
    int FireballCount = 0;  //현재 파이어볼 개수
    GameObject FireballHolder = null;   //생성된 파이어볼 담을 오브젝트

    int fireballMakeTime = 0;   //파이어볼 타이밍체크
    public int howManyFireball = 240; //파이어볼 생성 갯수
    public float Skill_1_RotateSpeed = 10f;  //파이어볼 생성할때 회전 속도


    public override void SetTarget()
    {
        AttackTarget = GameObject.FindObjectOfType<Player>() as BaseObject;
     
    }


    private void Awake()
    {
        //AttackTarget = GameObject.FindObjectOfType<Player>() as BaseObject;
        Prefab_Fireball = Resources.Load("Prefabs/Object/Skill/FireBall") as GameObject;
        Prefab_FireRing = Resources.Load("Prefabs/Object/Effect/" + "FireRing") as GameObject;
        List_Fireball = new List<Skill_Fireball>();
     
    }
    

    
    
    protected override IEnumerator Idle()
    {
        Stop();

        if ( AttackTarget!= null)
        {

            //거리가 1 이하면
            if (Vector3.Distance(AttackTarget.transform.position, LinkObject.transform.position) < attackRange)
            {
                Stop();
                transform.LookAt(AttackTarget.transform);
                AddNextAI(eAIStateType.Attack, AttackTarget);
            }
            else
            {

                //AddNextAI(eAIStateType.Attack,(BaseObject)AttackTarget, AttackTarget.transform.position);
                transform.LookAt(AttackTarget.transform);
                //SetMove(AttackTarget.transform.position); //목적지 설정
                AddNextAI(eAIStateType.Move);
            }

        }



       yield return StartCoroutine(base.Idle());
    }

    protected override IEnumerator Move()
    {

        //거리가 1 이하면
        if(Vector3.Distance(AttackTarget.transform.position, LinkObject.transform.position) < attackRange)
        {
            Stop();
            transform.LookAt(AttackTarget.transform);
            AddNextAI(eAIStateType.Attack,AttackTarget);
        }
        else
        {

            //AddNextAI(eAIStateType.Attack,(BaseObject)AttackTarget, AttackTarget.transform.position);
            transform.LookAt(AttackTarget.transform);
            SetMove(AttackTarget.transform.position); //목적지 설정
            AddNextAI(eAIStateType.Move);
        }
        
        
        //다음 업데이트 진행을 위해 BaseAI의 코루틴 동작
        yield return StartCoroutine(base.Move());
    }
    
    protected override IEnumerator Attack()
    {
        Stop();
        //공격 처리 들어가기 전의 잠깐의 텀을 준다
        //IsAttack = true;
        //yield return new WaitForEndOfFrame();

        while (IsAttack)
        {
            if (ObjectState == eBaseState.Die)
                break;



        


            yield return new WaitForEndOfFrame();
        }
        //공격 처리를 다했다면 다음은 기본상태로 돌아가게 명령
        AddNextAI(eAIStateType.Idle);
        
        //다음 업데이트 진행을 위해 BaseAI의 코루틴 동작
        yield return StartCoroutine(base.Attack());
    }

    protected override IEnumerator Die()
    {
        yield return StartCoroutine(base.Die());
    }





    IEnumerator Skill_1()
    {
        float FireballDistance = Random.Range(FireballInitDistance, FireballInitDistance+2.0f);
        ListNextAI.Clear();

        fireballMakeTime = 0;
        LinkObject.transform.LookAt(AttackTarget.transform);

        if(FireballHolder == null)
        {
            FireballHolder = new GameObject("FireballHolder");
            
        }

        b_Skill_1_ready = false;
        Ani.SetInteger("STATE", 1); // Idle 애니메이션

        //Storm이펙트 ON
        if (LinkObject.transform.Find("StormEffect").gameObject.activeSelf == false)
        {
            Transform stromEffect = LinkObject.transform.Find("StormEffect");
            stromEffect.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(3f);    //스킬 시전 전 3초대기

            //Quaternion rot_a = Quaternion.identity;
            //rot_a.eulerAngles = new Vector3(0, 0, 0);
            //Quaternion rot_b = Quaternion.identity;
            //rot_b.eulerAngles = new Vector3(0, 330, 0);
        while(true)
        {

            if (FireballCount >= howManyFireball)   //파이어볼이 전부 생성된뒤
            {
                Vector3 LookAtTarget = new Vector3(AttackTarget.transform.position.x, LinkObject.transform.position.y, AttackTarget.transform.position.z);

                LinkObject.transform.LookAt(LookAtTarget);
                //Ani.SetInteger("SKILL", 1);
                //EFFECT OFF
                if (LinkObject.transform.Find("StormEffect").gameObject.activeSelf == true)
                {
                    Transform stromEffect = LinkObject.transform.Find("StormEffect");
                    stromEffect.gameObject.SetActive(false);
                }

                if (List_Fireball.Count <= 0) // 생성된 파이어볼이 전부 디스트로이되어 파이어볼의 리스트의 파이어볼갯수가 0이 되었을때
                {
                    Destroy(FireballHolder);
                    Ani.SetInteger("SKILL", 0);
                    b_UpdateBaseAI = true;
                    IsAttack = false;
                    FireballCount = 0;
                    EffectInitCount = 0;
                    _CurrentState = eAIStateType.Idle;
                    yield break;
                }

                
            }

            else
            {
                //FireRing effect 생성
                if (EffectInitCount == 0)
                {
                    ParticleSystem particle = Prefab_FireRing.GetComponent<ParticleSystem>();
                    //particle.shape.radius = FireballInitDistance; 

                    Instantiate(Prefab_FireRing, LinkObject.transform.position + Vector3.up * 0.6f  ,Prefab_FireRing.transform.rotation);
                }
                EffectInitCount++;



                LinkObject.transform.Rotate(new Vector3(0, Skill_1_RotateSpeed, 0));    //드래곤 회전
            fireballMakeTime += (int)Skill_1_RotateSpeed;       //회전한만큼 fireballMakeTime 증가
            //LinkObject.transform.rotation = Quaternion.Euler(0,(360/howManyFireball) * FireballCount,0);
        Vector3 fireball_pos = LinkObject.transform.position + LinkObject.transform.forward * FireballDistance;
            if( fireballMakeTime >= (360/howManyFireball) * FireballCount)
            {
                GameObject go = Instantiate(Prefab_Fireball, fireball_pos, Quaternion.identity);
                List_Fireball.Add(go.transform.GetComponent<Skill_Fireball>());
                go.GetComponent<Skill_Fireball>().SetListFireBall(List_Fireball);
                go.transform.SetParent(FireballHolder.transform);
                    go.transform.rotation = Quaternion.Euler(0, 30 * FireballCount, 0);
                    FireballCount++;

            }
            

            }


            //go.transform.Rotate(new Vector3(0, 30 * FireballCount, 0));


            yield return new WaitForSeconds(0.01f);
        }


       
        

    }

    private void Update()
    {

        //스킬 1 쿨타임 계산
        if(!b_Skill_1_ready && b_UpdateBaseAI && List_Fireball.Count ==0)
        time += Time.deltaTime;

        if (time > 5f)
        {
            time = 0;
            b_Skill_1_ready = true;
            b_UpdateBaseAI = false;
            
        }

        if (b_Skill_1_ready)
            StartCoroutine(Skill_1());
        
        //BASEAI 업데이트 실행
        if(b_UpdateBaseAI)
        UpdateAI();

        
    }




    


    
}
