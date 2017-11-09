using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// IDLE = 1
/// MOVE = 2
/// ATTACK = 3
/// DASH = 4
/// SMITE = 5
/// WEAPONSKILL = 6
/// DAMAGE = 7
/// DIE = 8
/// 
/// Has Exit Time 이 켜져 있으면 해당 애니메이션을 다 실행하고 다음걸로 넘어간다.
/// Has Exit Time 이 꺼져 있으면 다음 애니메이션으로 바로 넘어간다.
/// 
/// SmiteEffect 변경
/// WeaponSkill 위치 변경
/// 
/// </summary>

public class BossPigAI : BaseAI
{
    GameObject Spear;

    GameObject Prefabs;
    Vector3 OriginPos;
    float AttackRange = 3;
    float Timer;
    float dist;
    public BaseObject target = null;

    // 스킬프리팹이 제대로 로드 됐는지 확인하는 불값
    bool SkillLoad;
    //====================
    // 스킬 실행 불변수
    bool SkillDash = true;
    bool SkillSmite = true;
    bool SkillWeapon = true;
    // 해당 스킬이 준비 되었을때 바로 스킬로 진입할 수 있는 불변수
    bool SkillReady = false;
    //================================================
    // 스킬 실행에 필요한 Time변수
    float SmiteTimer = 5f;
    float WeaponTimer = 5f;
    float DashTimer = 0f;
    // 대쉬스킬에서 방향 벡터를 구해줄 변수
    Vector3 Dir = new Vector3();
    Vector3 TargetPos = new Vector3();

    //====================
    GameObject skillHolder = null;

    public void Awake()
    {
        OriginPos = transform.position;
        //====================================
        // 스킬 로드

            SkillPrefabLoad();
        
        if (SkillLoad == true)
            Debug.Log(" 스킬프리팹 로드 성공");
        //=====================================
        //if (target != null)
        //target = GameObject.FindObjectOfType<Player>() as BaseObject;
    }

    public override void SetTarget()
    {
        if (target == null)
            target = GameObject.FindObjectOfType<Player>() as BaseObject;
    }

    private void Update()
    {
        // 스테이지 안에 들어왔을때냐 
        // 전투시작 됐을때냐
        Timer += Time.deltaTime;

        // IsSkill 상태일때 스킬 실행
            switch (attackType)
            {
                case AttackType.Normal_Attack:
                break;

                case AttackType.Dash_Attack:
                    {
                        Ani.SetInteger("SKILL", 4);
                        Dash();
                    }
                    break;
                case AttackType.Smite_Attack:
                    {
                        Ani.SetInteger("SKILL", 5);
                        Smite();
                    }
                    break;
                case AttackType.Weapon_Attack:
                    {
                    
                        Ani.SetInteger("SKILL", 6);
                        WeaponSkill();
                    }
                    break;
            }
        // IsSkill 상태가 아니면 그냥 일반 공격
        if(!IsSkill)
            UpdateAI();
    }

    protected override void ProcessIdle()
    {
        Ani.SetInteger("SKILL", 1);
        CurretnState = eAIStateType.Idle;
    }

    protected override void ProcessMove()
    {
        Ani.SetInteger("SKILL", 2);
        CurretnState = eAIStateType.Move;
    }

    protected override void ProcessAttack()
    {
        //if (10 <= Timer && SkillDash)
        //{
        //    SkillReady = true;
        //    attackType = AttackType.Dash_Attack;
        //}

        //if (20 <= Timer && SkillSmite)
        //{
        //    SkillReady = true;
        //    attackType = AttackType.Smite_Attack;
        //}

        if (3<= Timer && SkillWeapon)
        {
            SkillReady = true;
            attackType = AttackType.Weapon_Attack;
        }

        if (40 <= Timer)
        {
            SkillDash = true;
            SkillSmite = true;
            SkillWeapon = true;
            Timer = 0;
        }

        CurretnState = eAIStateType.Attack;
        Ani.SetInteger("SKILL", 3);
    }

    protected override void ProcessDie()
    {
        CurretnState = eAIStateType.Die;
        Ani.SetInteger("SKILL", 8);
    }

    protected override IEnumerator Idle()
    {
        dist = 0f;
        AttackRange = 5f;

        dist = Vector3.Distance(target.transform.position, LinkObject.transform.position);

        if (target != null)
        {
            if(SkillDash && SkillSmite && SkillWeapon)
            {
                if (dist > 20)
                {
                    AddNextAI(eAIStateType.Idle);
                }
            }

            if (dist <= 20 && dist > AttackRange)
            {
                AddNextAI(eAIStateType.Move, target);
            }

            if (SkillReady || dist < AttackRange)
            {
                AddNextAI(eAIStateType.Attack, target);
            }
        }
        yield return StartCoroutine(base.Idle());
    }

    protected override IEnumerator Move()
    {
        dist = Vector3.Distance(target.transform.position, LinkObject.transform.position);

        if (target != null)
        {
            if (SkillDash && SkillSmite && SkillWeapon)
            {
                if (dist > 20)
                {
                    AddNextAI(eAIStateType.Idle);
                }
            }

            if (dist <= 20)
            {
                MovePosition = target.transform.position;

                SetMove(MovePosition);

                AddNextAI(eAIStateType.Move, target);
            }

            if (SkillReady || dist < AttackRange)
            {
                AddNextAI(eAIStateType.Attack, target);
            }
        }

        yield return StartCoroutine(base.Move());
    }

    protected override IEnumerator Attack()
    {
        yield return new WaitForEndOfFrame();

        // 스킬 사용중일땐 IsSkill
        // IsSkill 이 false 상태가 되면 와일문 탈출
        // Animation 스크립트에서 IsSkil을 false로 만들어 
        // 와일문을 탈출한다.
        while (IsSkill || IsAttack)
        {
            if (LinkObject.transform.Find("AttackParticle").gameObject.activeSelf == false)
            {
                Transform SmiteParticle = LinkObject.transform.Find("AttackParticle");
                SmiteParticle.gameObject.SetActive(true);
            }

            yield return new WaitForEndOfFrame();
        }
        //=========================================
        if (LinkObject.transform.Find("AttackParticle").gameObject.activeSelf == true)
        {
            Transform SmiteParticle = LinkObject.transform.Find("AttackParticle");
            SmiteParticle.gameObject.SetActive(false);
        }

        AddNextAI(eAIStateType.Idle);
        yield return StartCoroutine(base.Attack());
    }

    protected override IEnumerator Die()
    {
        bEnd = true;
        yield return StartCoroutine(base.Die());
    }

    //====================================================================
    // 스킬 프리팹 로드
    void SkillPrefabLoad()
    {
        Spear = Resources.Load("Prefabs/Object/Skill/Weapon_2") as GameObject;

        if (Spear == null)
        {
            Debug.Log(Prefabs.name + " 로드 실패");
        }

        SkillLoad = true;
    }
    //====================================================================
    // SpearSkill

    void WeaponSkill()
    {
        Vector3 CreatPos = new Vector3();

        if (LinkObject.transform.Find("WeaponParticle").gameObject.activeSelf == false)
        {
            Transform SmiteParticle = LinkObject.transform.Find("WeaponParticle");
            SmiteParticle.gameObject.SetActive(true);
        }

        if (WeaponTimer == 5)
        {
            TargetPos.x = target.transform.position.x;
            TargetPos.y = target.transform.position.y - 5f;
            TargetPos.z = target.transform.position.z;
            CreatPos = new Vector3(TargetPos.x, TargetPos.y, TargetPos.z);
        }

        SkillWeapon = false;

        if (skillHolder == null)
        {
            skillHolder = new GameObject();
            skillHolder.name = "Spearholder";
            // LinkObject를 부모설정
            // 월드포지션 그대로 사용하겠다고 false값 설정
            skillHolder.transform.SetParent(LinkObject.transform, false);
        }

        WeaponTimer -= Time.deltaTime;

        // 해당 메서드 안에 들어왔을때 한번만 실행하고 탈출하기 위해 attackType를 바꿔준다.

        if (WeaponTimer <= 4 && WeaponTimer >= 3.8)
        {
            for (int j = 0; j < 3; j++)
            {
                Prefabs = Instantiate(Spear, CreatPos + RandomPos(), Quaternion.identity);

                if (skillHolder != null)
                {
                    Prefabs.transform.SetParent(skillHolder.transform, false);
                }
            }
        }

        if (WeaponTimer < 1)
        {
            if (LinkObject.transform.Find("WeaponParticle").gameObject.activeSelf == true)
            {
                Transform SmiteParticle = LinkObject.transform.Find("WeaponParticle");
                SmiteParticle.gameObject.SetActive(false);
            }

            attackType = AttackType.Normal_Attack;

            SkillReady = false;
            // 리스트 삭제
            //int index = SkillList.IndexOf(SkillList[0]);

            //for(int i = 0; i < SkillList.Count; i++)
            //{
            //    SkillList.RemoveAt(i);
            //}
            WeaponTimer = 5f;
            AddNextAI(eAIStateType.Idle);
        }
    }

    //====================================================================
    // Smite스킬
    void Smite()
    {
        if (SmiteTimer == 5)
        {
            if (LinkObject.transform.Find("SmiteParticle").gameObject.activeSelf == false)
            {
                Transform SmiteParticle = LinkObject.transform.Find("SmiteParticle");
                SmiteParticle.gameObject.SetActive(true);
            }
        }

        SmiteTimer -= Time.deltaTime;

        //if (SmiteTimer <= WaitingSmite)
        //{
        //    GameObject Weapon = GameObject.Find("Weapon_1") as GameObject;

        //    Weapon.GetComponentInParent<BoxCollider>().size = new Vector3(2.5f, 0.5f, 2.5f);

        //    GameObject.Find("Weapon_1").GetComponentInChildren<BoxCollider>().Raycast(ray, )
        //}

        if (SmiteTimer < 1)
        {
            if (LinkObject.transform.Find("SmiteParticle").gameObject.activeSelf == true)
            {
                Transform SmiteParticle = LinkObject.transform.Find("SmiteParticle");
                SmiteParticle.gameObject.SetActive(false);
            }

            attackType = AttackType.Normal_Attack;
            SkillSmite = false;
            SkillReady = false;
            SmiteTimer = 5f;
            //GameObject.Find("Weapon_1").GetComponentInParent<BoxCollider>().size = new Vector3(0.055f, 0.68f, 0.054f);
            AddNextAI(eAIStateType.Idle);
        }
    }
    //====================================================================
    // Dash스킬

    void Dash()
    {
        if(DashTimer == 0)
        {
            LinkObject.transform.LookAt(target.transform);

            Dir = (target.transform.position - LinkObject.transform.position).normalized;

            if (LinkObject.transform.Find("DashParticle").gameObject.activeSelf == false)
            {
                Transform DashParticle =LinkObject.transform.Find("DashParticle");
                DashParticle.gameObject.SetActive(true);
            }
        }

        float WaitingDash = 3;
        DashTimer += Time.deltaTime;

            //LinkObject.transform.LookAt(target.transform);
        float MoveSpeed = 1f;
        NavAgent.isStopped = true;
        //대쉬할 방향
        
        Vector3 Dest = LinkObject.transform.position + Dir * MoveSpeed; // 대쉬 최종목적지
        LinkObject.transform.position = Vector3.Lerp(LinkObject.transform.position, Dest, DashTimer * 0.1f);
        //transform.position += Dir * MoveSpeed * Time.deltaTime;



        //transform.LookAt(target.transform);
        //transform.position = 
        //Vector3 destPos = (target.transform.position).normalized;

        //    //// 애니메이션 실행
        // NavAgent.SetDestination(destPos);
        // NavAgent.acceleration = 5;
        // NavAgent.speed = 15;

        //this.GetComponentInParent<BoxCollider>().size = new Vector3(5f, 2f, 5f);

        // 왜 쓴건지 기억해
        //if()

        if (WaitingDash < DashTimer)
        {
            if (LinkObject.transform.Find("DashParticle").gameObject.activeSelf == true)
            {
                Transform DashParticle = LinkObject.transform.Find("DashParticle");
                DashParticle.gameObject.SetActive(false);
            }

            attackType = AttackType.Normal_Attack;
            SkillDash = false;
            SkillReady = false;
            // this.GetComponentInParent<BoxCollider>().size = new Vector3(1f, 1f, 1f);
            // AddNextAI(eAIStateType.Idle);
            DashTimer = 5f;
            AddNextAI(eAIStateType.Idle);
        }
    }

    Vector3 RandomPos()
    {
        Vector3 Dir = new Vector3(Random.Range(-1f, 1f), -3, Random.Range(-1f, 1f));
        return Dir.normalized * Random.Range(1, 5);
    }
    //====================================================================

     //Dash 상태일때 벽과 충돌시 IsSkill 을 종료 시킨다.
     //스크립트 합친 다음에 실험
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(LinkObject.transform.attackType == AttackType.Dash_Attack)
    //    {
    //        if(other.tag == "Wall")
    //        {
    //            IsSkill = false;
    //            AddNextAI(eAIStateType.Idle);
    //        }
    //    }
    //}

    //void Looktarget(bool islook)
    //{
    //    if(islook == true)
    //    {
    //        LinkObject.transform.LookAt(target.transform);
    //    }
    //    else
    //    {

    //    }
    //}
}
