using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : BaseObject
{

    bool isPlayer = false;
    public bool IsPlayer        // 내가 주인공이다!!!
    {
        get { return isPlayer; }
        set { isPlayer = value; }
    }

    [SerializeField]
    eTeamType teamType;
    public eTeamType TeamType
    {
        get { return teamType; }
        set { teamType = value; }
    }

    BaseObject AttackTarget = null;

    [SerializeField]
    eAIType _AIType = eAIType.AI_NONE;
    public eAIType AIType
    {
        get { return _AIType; }
    }

    eActorType _ActorType;
    public eActorType ActorType
    {
        get { return _ActorType; }
        set { _ActorType = value; }
    }

    BaseAI _AI = null;
    public BaseAI AI
    {
        get { return _AI; }
    }

    Actor _Target = null;
    public Actor Target
    {
        get { return _Target; }
        set { _Target = value; }
    }
        

    virtual protected void Awake()
    {
        Initialize();
    }

    virtual protected void Update()
    {
       // AI.UpdateAI();
    }

    void Initialize()
    {
        if (IsPlayer)
        {
            ActorManager.Inst.AddActor(this);
            return;
        }

        switch (AIType)
        {
            case eAIType.AI_NONE:
                break;
            case eAIType.AI_Normal:
                {
                    GameObject ai = new GameObject(AIType.ToString(), typeof(NormaAI));
                    ai.transform.SetParent(SelfTransform);
                    _AI = ai.GetComponent<NormaAI>();
                }
                break;
            case eAIType.AI_DRAGON:
                {
                    GameObject ai = new GameObject(AIType.ToString(), typeof(BossDragonAI));
                    ai.transform.SetParent(SelfTransform);
                    _AI = ai.GetComponent<BossDragonAI>();
                }
                break;
            case eAIType.AI_PIG:
                {
                    GameObject ai = new GameObject(AIType.ToString(), typeof(BossPigAI));
                    ai.transform.SetParent(SelfTransform);
                    _AI = ai.GetComponent<BossPigAI>();
                }
                break;
            default:
                break;
        }

        if (AIType != eAIType.AI_NONE)
            AI.LinkObject = this;

        ActorManager.Inst.AddActor(this);
    }

    public override object GetData(string keyData, params object[] datas)
    {
        switch(keyData)
        {
            case ConstValue.ActorData_GetTarget:
                return AttackTarget;                
            case ConstValue.ActorData_GetTeam:
                return TeamType;
            default:
                return base.GetData(keyData, datas);
                
        }
    }

    public override void ThrowEvent(string keyData, params object[] datas)
    {
        switch(keyData)
        {
            case ConstValue.ActorData_SetTarget:
                {
                    AttackTarget = datas[0] as BaseObject;
                }
                break;
        }
    }
}
