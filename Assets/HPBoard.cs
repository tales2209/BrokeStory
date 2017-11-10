using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBoard : MonoSingleton<HPBoard>
{
    public UIProgressBar uIProgressBar = null;
    public UILabel uILabel = null;

    // Camera 변수 2개를 생성한 후 한개에는 UICamera를 붙여주고
    // 나머지 한개에는 WorldCamera를 붙여준다.
    Camera UICam = null;
    Camera WorldCam = null;

    Transform HPBar = null;
    Vector3 position = Vector3.zero;

    //private void Start()
    //{
    //    HPBar.position = CreatePos();
    //}

    public Camera uICamera
    {
        get
        {
            if(UICam == null)
            {
                UICam = UICamera.mainCamera;
            }
                return UICam;
        }
    }

    public Camera WorldCamera
    {
        get
        {
            if(WorldCam == null)
            {
                WorldCam = Camera.main;
            }
            return WorldCam;
        }
    }


    public void HPBarPrefabLoad()
    {
        if(HPBar == null)
        {
            GameObject go = Resources.Load("UI/" + "HPBoard") as GameObject;
        }
    }


}
