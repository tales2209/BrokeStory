using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillCollider : MonoBehaviour
{
    Actor Owner;
    BoxCollider attackCollider;

    private void Awake()
    {
        Owner = GetComponentInParent<Actor>();
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
        if (Owner.AI.IsAttack || Owner.AI.IsSkill)
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
        Vector3 collPos = attackCollider.transform.position;

        //AI가 공격 행동 중이고
        if (Owner.AI.IsAttack || Owner.AI.IsSkill)
        {
            if(Owner.AI.attackType == AttackType.Smite_Attack)
            {
                collPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
            }

            if(Owner.AI.attackType == AttackType.Weapon_Attack)
            {
                attackCollider.size = new Vector3(8f, 8f, 8f);
            }

            //충돌체가 플레이어이면
            if (other.transform.GetComponent<Actor>().IsPlayer)
                other.transform.GetComponent<Actor>().ThrowEvent(ConstValue.EventKey_Hit);
        }
    }
}
