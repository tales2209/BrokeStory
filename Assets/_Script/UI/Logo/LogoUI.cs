using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoUI : BaseObject
{
    UIButton StartButton = null;

    private void Start()
    {
        Transform temp = this.Getchild("StartButton");
        if(temp == null)
        {
            Debug.Log("LogoUI에 StartButton 이 없습니다.");
            return;
        }

        StartButton = temp.GetComponent<UIButton>();

        EventDelegate.Add(StartButton.onClick, () => { Scene_Manager.Inst.LoadScene(eSceneType.BattleScene); });
    }
}
