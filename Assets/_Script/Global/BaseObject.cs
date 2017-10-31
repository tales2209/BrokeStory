using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    BaseObject _LinkObject = null;
    public BaseObject LinkObject
    {
        get { return _LinkObject; }
        set { _LinkObject = value; }
    }

    eBaseState _ObjectState = eBaseState.Normal;
    public eBaseState ObjectState
    {
        get { return _ObjectState; }
        set { _ObjectState = value; }
    }

    virtual public object GetData(string keyData, params object[] datas)
    {
        return null;
    }

    virtual public void ThrowEvent(string keyData, params object[] datas)
    {

    }

    public GameObject SelfObject
    {
        get
        {
            if (_LinkObject == null)
                return this.gameObject;
            else
                return _LinkObject.gameObject;
        }    

    }

    public Transform SelfTransform
    {
        get
        {
            if (_LinkObject == null)
                return this.transform;
            else
                return _LinkObject.transform;
        }

    }

    public Transform Getchild(string strName)
    {
        return _GetChild(strName, SelfTransform);
    }
    
    private Transform _GetChild(string strName, Transform trans)
    {
        if (trans.name == strName)
            return trans;

        for(int i=0; i< trans.childCount; ++i)
        {
            Transform returnTrans = _GetChild(strName, trans.GetChild(i));

            if (returnTrans != null)
                return returnTrans;
        }

        return null;
    }
}
