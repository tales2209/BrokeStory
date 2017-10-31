using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    static bool bShotDown = false;
    private static T _inst = null;

    public static T Inst
    {
        get
        {
            if (_inst == null)
            {
                if (bShotDown == false)
                {
                    T inst = GameObject.FindObjectOfType<T>() as T;

                    if(inst == null)
                    {
                        inst = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    }

                    InstanceInit(inst);
                }
            }

            return _inst;
        }

    }

    private static void InstanceInit(Object obj)
    {
        _inst = obj as T;
        _inst.Init();
    }

    public virtual void Init()
    {
        DontDestroyOnLoad(_inst);
    }

    public virtual void OnDestroy()
    {
        _inst = null;
        bShotDown = true;

    }


}
