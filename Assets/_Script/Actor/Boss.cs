using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Actor
{
    public override void ThrowEvent(string keyData, params object[] datas)
    {
        switch(keyData)
        {
            case ConstValue.EventKey_Hit:
                {
                    Hit();
                }
                break;
        }

        base.ThrowEvent(keyData, datas);
    }

    void Hit()
    {




    }




}
