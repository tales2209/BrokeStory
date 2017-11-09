using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUI : BaseObject
{
    UIProgressBar ProgressBar;

    private void OnEnable()
    {
        if (ProgressBar == null)
            ProgressBar = this.GetComponentInChildren<UIProgressBar>();
    }

    public override void ThrowEvent(string keyData, params object[] datas)
    {
        if (keyData.Equals("LoadingValue"))
        {
            ProgressBar.value = (float)datas[0];
        }
        else
            base.ThrowEvent(keyData, datas);
    }
}
