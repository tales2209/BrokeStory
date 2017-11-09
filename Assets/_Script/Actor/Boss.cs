using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Actor
{
    bool b_hitting = false;
    public override void ThrowEvent(string keyData, params object[] datas)
    {
        switch(keyData)
        {
            case ConstValue.EventKey_Hit:
                {
                    if(!b_hitting)
                    StartCoroutine(Hit());
                }
                break;
        }

        base.ThrowEvent(keyData, datas);
    }

    IEnumerator Hit()
    {
        b_hitting = true; //맞는거 프로세스 중에는 중첩해서 데미지입게 안할꺼

        AI.ChangeAnimation(eAIStateType.Hit);   //맞는 애니메이션
        AI.Stop();                              //움직임 멈춤
        AI._bUpdateAI = true;                   //AI 업데이트 멈춤




        yield return new WaitForSeconds(3f);    // 1초대기

        AI.ChangeAnimation();   //현재 상태에 맞는 애니메이션 다시 실행
        AI._bUpdateAI = false; //업데이트 다시 진행




        b_hitting = false;
        yield return null;
    }




}
