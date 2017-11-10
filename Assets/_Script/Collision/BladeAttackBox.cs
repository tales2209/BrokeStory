using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeAttackBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HitBox")
        {
            Actor enemy = other.GetComponentInParent<Actor>();
            enemy.ThrowEvent(ConstValue.EventKey_Hit);

            EffectManager.Inst.AutoPlayEffect("Ice_Shatter", this.transform.parent, false);
        }
    }    
}
