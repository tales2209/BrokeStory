using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "HitBox")
        {
            Actor enemy = other.GetComponentInParent<Actor>();
            enemy.ThrowEvent(ConstValue.EventKey_Hit);
            //Debug.Log("플레이어가 몹 때림 으앙");
        }
    }
}
