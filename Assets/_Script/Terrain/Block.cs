using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    BoxCollider Coll = null;
    Camera MainCamera = null;

    private void Start()
    {
        Coll = GetComponent<BoxCollider>();
        MainCamera = Camera.main;
    }

    void Update()
    {
        Picking();
    }

    void Picking()
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float dist = 100;

        if(Coll.Raycast(ray,out hit, dist))
        {
            Debug.Log(hit.point);
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
    }
}
