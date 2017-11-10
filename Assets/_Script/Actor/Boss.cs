using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Actor
{
    int HP = 10;
    bool b_hitting = false;
    GameObject Hit_Particle;

    private void Start()
    {
        Hit_Particle = Resources.Load("Prefabs/Object/Effect/" + "HitEffect")as GameObject;
    }

    public override void ThrowEvent(string keyData, params object[] datas)
    {
        switch(keyData)
        {
            case ConstValue.EventKey_Hit:
                {
                    GameObject HitPos = datas[0] as GameObject;
                    if(!b_hitting)
                    StartCoroutine(Hit(HitPos));
                }
                break;
        }

        base.ThrowEvent(keyData, datas);
    }

    IEnumerator Hit(GameObject HitPos)
    {
        b_hitting = true; //맞는거 프로세스 중에는 중첩해서 데미지입게 안할꺼

        //히트 이펙트 생성
        //Vector3 InstPos = new Vector3(transform.position.x ,transform.position.y + Vector3.up * 0.5f ,  )
        if (HitPos == null)
            Debug.Log("HitPos가 널이야 친구야");
        Instantiate(Hit_Particle, HitPos.transform.position, Hit_Particle.transform.rotation);

        //HP 깎음
        HP--;
        if(HP == 0)
        {
            AI.ChangeAnimation(eAIStateType.Die);
        }

        AI.Stop();                              //움직임 멈춤
        AI._bUpdateAI = true;                   //AI 업데이트 멈춤
        if(HP != 0)
        AI.ChangeAnimation(eAIStateType.Hit);   //맞는 애니메이션




        yield return new WaitForSeconds(1f);    // 1초대기

        if (HP == 0)
            Destroy(gameObject);

        AI.ChangeAnimation();   //현재 상태에 맞는 애니메이션 다시 실행
        AI._bUpdateAI = false; //업데이트 다시 진행




        b_hitting = false;

        yield return null;
    }




}
