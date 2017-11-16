using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : StateMachineBehaviour
{
    Actor TargetActor;
    bool bIsAttack = false;

    public override void OnStateEnter(
        Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // TargetActor = animator.transform.parent.GetComponent<Actor>();
        TargetActor = animator.GetComponentInParent<Actor>();

        if (TargetActor != null
            && TargetActor.AI.CurretnState == eAIStateType.Attack)
        {
            TargetActor.AI.IsAttack = true;
            TargetActor.AI.IsSkill = true;
            bIsAttack = false;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // animatorStateInfo.normalizedTime 
        // 애니메이션은 0에서 1의 값으로 가면 한 주기
        // n초와 상관없이 1번 실행됐다는걸 판가름 해준다.

        // 애니메이션이 끝날즈음 콜라이더 키워주고

        // 끝날때 다시 콜라이더를 줄여준다.
        if (animatorStateInfo.normalizedTime >= 1f
            && TargetActor.AI.IsSkill)
        {
            if (TargetActor.AI.CurretnState == eAIStateType.Attack)
            {
                // 애니메이션이 종료되는 시점과 내 공격 or 스킬이 종료되는 시점을 맞춰준다.
                TargetActor.AI.IsSkill = false;
                TargetActor.AI.IsAttack = false;
            }
        }

        if (bIsAttack == false
            && animatorStateInfo.normalizedTime >= 0.5f)
        {
            bIsAttack = true;
            
            // 스킬을 쓰는경우에는 여기에 스킬 메써드 실행
        }
    }

}
