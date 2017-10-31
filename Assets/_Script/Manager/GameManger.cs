using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoSingleton<GameManger>
{
    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {        
        GameObject go = ActorManager.Inst.GetPrefab(eActorType.Player);
        Actor player = ActorManager.Inst.InstantiateActor(go, Vector3.up / 2);        
    }
}
