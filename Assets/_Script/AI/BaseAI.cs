using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NextAI
{
    public eAIStateType State;
    public BaseObject Target;
    public Vector3 TargetPosition;
}

public class BaseAI : BaseObject
{
    //BossPigAI에서 사용
    public AttackType attackType = AttackType.Normal_Attack;
    bool _IsSkill = false;
    public bool IsSkill
    {
        get { return _IsSkill; }
        set { _IsSkill = value; }
    }

    protected List<NextAI> ListNextAI = new List<NextAI>();

    protected eAIStateType _CurrentState = eAIStateType.Idle;
    public eAIStateType CurretnState
    {
        get { return _CurrentState; }
        set { _CurrentState = value; }
    }

    bool bUpdateAI = false;
    public bool _bUpdateAI
    {
        get { return bUpdateAI; }
        set { bUpdateAI = value; }
    }

    bool _IsAttack = false;
    public bool IsAttack
    {
        get { return _IsAttack; }
        set { _IsAttack = value; }
    }

    bool _bEnd = false;
    public bool bEnd
    {
        get { return _bEnd; }
        set { _bEnd = value; }
    }

    protected Vector3 MovePosition = Vector3.zero;
    Vector3 PreMovePosition = Vector3.zero;

    Animator _Ani = null;
    public Animator Ani
    {
        get
        {
            if (_Ani == null)
                _Ani = SelfObject.GetComponentInChildren<Animator>();

            return _Ani;
        }
    }

    
    NavMeshAgent _NavAgent = null;
    public NavMeshAgent NavAgent
    {
        get
        {
            if(_NavAgent == null)
            {
                _NavAgent = SelfObject.GetComponent<NavMeshAgent>();
            }

            return _NavAgent;
        }
    }

    protected bool MoveCheck()
    {
        // 현재 경로(path)의 상태가 해당 경로에 도착(Complete)했다면
        if(NavAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            //현재 경로가 없거나 경로를 기다리는 중이라면
            if(NavAgent.hasPath == false || NavAgent.pathPending == false)
            {
                return true; // 목적지에 도착했고 다음 경로가 없거나 경로를 기다리는 중이라면 
            }
        }

        return false;
    }

    public virtual void SetTarget()
    {

    }


    protected void SetMove(Vector3 dest)
    {
        dest.y = LinkObject.transform.position.y;

        if (PreMovePosition != dest)
        {

        NavAgent.isStopped = false;
        NavAgent.SetDestination(dest);        
        PreMovePosition = dest;




        }

    }

    public void Stop()
    {
        MovePosition = Vector3.zero;
        NavAgent.isStopped = true;
    }

    protected virtual void ProcessIdle()
    {
        _CurrentState = eAIStateType.Idle;
        ChangeAnimation();
    }

    protected virtual void ProcessMove()
    {
        _CurrentState = eAIStateType.Move;
        ChangeAnimation();
    }

    protected virtual void ProcessAttack()
    {
        _CurrentState = eAIStateType.Attack;
        ChangeAnimation();
    }

    protected virtual void ProcessDie()
    {
        _CurrentState = eAIStateType.Die;
        ChangeAnimation();
    }

    protected virtual IEnumerator Idle()
    {
        bUpdateAI = false;
        yield break;
    }

    protected virtual IEnumerator Move()
    {
        bUpdateAI = false;
        yield break;
    }

    protected virtual IEnumerator Attack()
    {
        bUpdateAI = false;
        yield break;
    }

    protected virtual IEnumerator Die()
    {
        bUpdateAI = false;
        yield break;
    }

    public virtual void AddNextAI(eAIStateType nextState, BaseObject targetObject = null, Vector3 position = new Vector3())
    {
        NextAI nextAI = new NextAI
        {
            State = nextState,
            Target = targetObject,
            TargetPosition = position
        };

        ListNextAI.Add(nextAI);
    }

    //AI 동작하기 위한 데이터를 셋팅
    void SetNextAI(NextAI nextAI)
    {
        if(nextAI.Target != null)
        {
            LinkObject.ThrowEvent(ConstValue.ActorData_SetTarget, nextAI.Target);
        }

        if(nextAI.TargetPosition != Vector3.zero)
        {
            MovePosition = nextAI.TargetPosition;
        }

        switch (nextAI.State)
        {
            case eAIStateType.Idle:
                ProcessIdle();
                break;
            case eAIStateType.Move:
                ProcessMove();               
                break;
            case eAIStateType.Attack:
                //TargetLookAt(nextAI.Target);
                ProcessAttack();
                break;
            case eAIStateType.Die:
                ProcessDie();
                break;
        }

    }

    void TargetLookAt(BaseObject target)
    {
        if(target != null)
        {
            SelfTransform.forward = (target.SelfTransform.position - SelfTransform.position).normalized;
        }
    }

    // 세팅된 데이터를 기반으로 동작을 실행
    public void UpdateAI()
    {
        if (bEnd == true)
            return;

        if (bUpdateAI == true)
            return;

        if(ListNextAI.Count > 0)
        {
            SetNextAI(ListNextAI[0]);
            ListNextAI.RemoveAt(0);
        }

        if(ObjectState == eBaseState.Die)
        {
            ListNextAI.Clear();
            ProcessDie();
            return;
        }

        bUpdateAI = true;

        switch (CurretnState)
        {            
            case eAIStateType.Idle:
                StartCoroutine("Idle");
                break;
            case eAIStateType.Move:
                StartCoroutine("Move");
                break;
            case eAIStateType.Attack:
                StartCoroutine("Attack");
                break;
            case eAIStateType.Die:
                StartCoroutine("Die");
                break;
        }
    }

    public void ChangeAnimation(eAIStateType AIState = eAIStateType.None)
    {
        if(AIState != eAIStateType.None)    //매개변수 주어졌다면 현재상태 바꾸지않고 애니메이션만 해당 enum값에 해당하는걸로 바꿈
        {
        Ani.SetInteger("STATE", (int)AIState);

        }
        else
        {
            Ani.SetInteger("STATE", (int)_CurrentState);
        }
    }
}