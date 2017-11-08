public enum eTeamType
{
    Red,
    Blue,
}

public enum eBaseState
{
    Normal,
    Die
}
public enum eActorType
{
    Player,
    Boss_Dragon,
    //Shooter,
    Max
}

public enum ePlayerState
{
    AttackReady, //0
    Run, //1
    Jump, //2
    Wound, //3
    HitAway, //4
    HitAwayUp, //5
    Attack1, //6
    Attack2, //7
    Attack3_1, //8
    Attack3_2, //9
    Attack3_3, //10 
    Attack4, //11
    Magic, //12
    Fire, //13
    Death, //14    
    none
}
public enum eAIStateType
{
    None,
    Idle,
    Move,
    Attack,
    Skill1,
    Die
}

public enum eAIType
{
    AI_NONE,
    AI_Normal,
    AI_DRAGON,
    AI_PIG,
}


//BossPigAI 에서 사용
public enum AttackType
{
    Normal_Attack,
    Dash_Attack,
    Smite_Attack,
    Weapon_Attack,
}


public enum eRegenType
{
    None,
    Time,
    Triger
}