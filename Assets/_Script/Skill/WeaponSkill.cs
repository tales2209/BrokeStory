using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSkill : MonoBehaviour
{
    private void Update()
    {
        Invoke("SpearUp", 3);
    }

    void SpearUp()
    {
        transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);

        // 프리팹이 로드후 성생되면서 프리팹에 스크립트가 달려 있기 때문에
        // 프리팹의 스크립트가 자동 활성화 되면서 
        // 프리팹의 스크립트에서 삭제를 진행해주면 된다.

        Destroy(this.gameObject, 1f);
    }
}
