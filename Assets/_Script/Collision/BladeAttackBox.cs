using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeAttackBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HitBox")
        {
            Debug.Log("검 스킬이 후둘겨 팹니다");
        }
    }    
}
