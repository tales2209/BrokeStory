using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormaAI : BaseAI
{
    float AttackRange = 3;

    protected override IEnumerator Idle()
    {
        float dist = 0;

        BaseObject target = ActorManager.Inst.GetSerchEnemy(LinkObject, out dist);

        if(target != null)
        {            
            if(dist < AttackRange)
            {
                Stop();
                AddNextAI(eAIStateType.Attack, target, target.SelfTransform.position);
            }
            else
            {
                AddNextAI(eAIStateType.Move, target, target.SelfTransform.position);
            }
        }

        yield return StartCoroutine(base.Idle());        
    }

    protected override IEnumerator Move()
    {
        float dist = 0;

        BaseObject target = ActorManager.Inst.GetSerchEnemy(LinkObject, out dist);

        if (target != null)
        {
            if(dist < AttackRange)
            {
                Stop();
                AddNextAI(eAIStateType.Attack, target, target.transform.position);
            }
            else
            {
                SetMove(target.transform.position);
            }
        }

        yield return StartCoroutine(base.Move());
    }

    protected override IEnumerator Attack()
    {
        float dist = 0;

        BaseObject target = ActorManager.Inst.GetSerchEnemy(LinkObject, out dist);

        if (target != null)
        {
            if (dist < AttackRange)
            {
                Stop();
                AddNextAI(eAIStateType.Attack, target, target.SelfTransform.position);
            }
            else
            {
                AddNextAI(eAIStateType.Move, target, target.SelfTransform.position);
            }
        }
        yield return base.Attack();
    }

}
