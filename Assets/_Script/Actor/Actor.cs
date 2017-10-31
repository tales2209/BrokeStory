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

    eAIType _AIType = eAIType.Normal;
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

    virtual protected void Awake()
    {
        Initialize();
    }

    virtual protected void Update()
    {
        AI.UpdateAI();
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
            case eAIType.Normal:
                {
                    GameObject ai = new GameObject(AIType.ToString(), typeof(NormaAI));
                    ai.transform.SetParent(SelfTransform);
                    _AI = ai.GetComponent<NormaAI>();
                }
                break;
        }

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
