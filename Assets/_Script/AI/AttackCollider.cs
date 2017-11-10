using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    Actor Owner;
    BoxCollider attackCollider;

    private void Awake()
    {
        Owner = GetComponentInParent<Boss>();
        attackCollider = GetComponent<BoxCollider>();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //AI가 공격 행동 중이고
    //if(Owner.AI.IsAttack)
    //    {
    //        //충돌체가 플레이어이면
    //        if(collision.transform.GetComponent<Actor>().IsPlayer)
    //        collision.transform.GetComponent<Actor>().ThrowEvent(ConstValue.EVENTKEY_HIT);

    //    }

    //}
    private void Update()
    {
        if(Owner.AI.IsAttack)
        {
            attackCollider.enabled = true;
        }
        else
        {
            attackCollider.enabled = false;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        //AI가 공격 행동 중이고
        if (Owner.AI.IsAttack)
        {
            if(other.gameObject.tag == "HitBox")
            {
            //충돌체가 플레이어이면
            if (other.transform.GetComponentInParent<Actor>().IsPlayer)
                other.transform.GetComponentInParent<Actor>().ThrowEvent(ConstValue.EventKey_Hit);

            }

        }
    }







}
