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

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        GameObject go = ActorManager.Inst.GetPrefab(eActorType.Player);
        Actor player = ActorManager.Inst.InstantiateActor(go, StartPoint.transform.position);

        //go = ActorManager.Inst.GetPrefab(eActorType.Boss_Pig);
        //Actor Enemy = ActorManager.Inst.InstantiateActor(go, EnemyPoint.transform.position);

        go = ActorManager.Inst.GetPrefab(eActorType.Boss_Dragon);
        Actor  Enemy_2 = ActorManager.Inst.InstantiateActor(go, EnemyPoint.transform.position);

        GameObject[] EnemyGo = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < EnemyGo.Length; i++)
        {
            if (EnemyGo[i].GetComponent<Actor>().AI != null)
                EnemyGo[i].GetComponent<Actor>().AI.SetTarget();
        }

        // 이펙트매니저 초기화 
        EffectLoad();
    }

    void EffectLoad()
    {
        EffectManager.Inst.LoadEffect("Prefabs/Object/Effect/Player/", "BladeWork");
        EffectManager.Inst.LoadEffect("Prefabs/Object/Effect/Player/", "Orb");
        EffectManager.Inst.LoadEffect("Prefabs/Object/Effect/Player/", "Ice_Shatter");
    }

    public void LoadBattle()
    {
        UITools.Inst.ShowUI(eUIType.BattleUI);
    }

    public void DisableBattle()
    {
        UITools.Inst.HideUI(eUIType.BattleUI);
    }
}
