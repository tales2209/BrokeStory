using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoSingleton<GameManger>
{
    public Transform StartPoint;
    public Transform EnemyPoint;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {        
        GameObject go = ActorManager.Inst.GetPrefab(eActorType.Player);
        Actor player = ActorManager.Inst.InstantiateActor(go, StartPoint.transform.position);
        go = ActorManager.Inst.GetPrefab(eActorType.Enemy);
        Actor Enemy = ActorManager.Inst.InstantiateActor(go, EnemyPoint.transform.position);
    }
}
