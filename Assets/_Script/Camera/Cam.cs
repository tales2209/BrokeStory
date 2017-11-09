using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoSingleton<Cam>
{    
    Transform Target = null;
    Vector3 OriginPos;

    [Range(-50, 100)]
    public float Distance = 0;
    [Range(-50, 100)]
    public float Height = 0;
    [Range(-50, 100)]
    public float Far = 0;
    


    //private void Start()
    //{
    //    Target = ActorManager.Inst.GetPlayer().transform;
    //}

    private void LateUpdate()
    {
        FollowCamera();   
    }

    public void FollowCamera()
    {
        if (Target == null)
        {
            Target = ActorManager.Inst.GetPlayer().transform;
        }

        Vector3 targetPos = Target.position;
        OriginPos = new Vector3(0, Height, -Distance);

        this.transform.position = targetPos + (this.transform.forward + OriginPos);
        this.transform.LookAt(targetPos + (Vector3.forward * Far));
        Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
    }

}
