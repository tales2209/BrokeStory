using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 씬매니저에서는 아래 using 지시문이 추가되어야 한다.
using UnityEngine.SceneManagement;


public class Scene_Manager : MonoSingleton<Scene_Manager>
{
    // 비동기 방식
    // 비동기 방식이란 로직이 로딩을 실행하고 있을때, 비동기 상태로 함으로써
    // 유저에게 로딩중이다 라는 별개의 메세지를 날려줄 수 있고,
    // 동기 방식은 로딩중인 상태에 내가 맞춰짐으로 인해 
    // 유저에게 다른 메세지를 날려줄 시간적 여유가 존재하지 않는다.

    // 비동기 방식
    bool IsAsyc = true;
    AsyncOperation Operation = null;

    eSceneType CurrentType = eSceneType.LogoScene;
    eSceneType NextType = eSceneType.None;
    
    public void LoadScene(eSceneType type, bool isAsyc = true)
    {
        if (CurrentType == type)
            return;

        NextType = type;
        IsAsyc = isAsyc;
    }

    private void Update()
    {
        if(Operation != null)
        {
            UITools.Inst.ShowLoadingUI(Operation.progress);
            UITools.Inst.SubRootCreate();
            if (Operation.isDone == true)
            {
                CurrentType = NextType;
                ComplateLoad(CurrentType);

                // 오퍼레이션 & 넥스트타입 초기화
                Operation = null;
                NextType = eSceneType.None;
                UITools.Inst.HideUI(eUIType.LoadingUI, true);
            }
            else
                return;
        }

        if (CurrentType == eSceneType.None)
            return;

        if(NextType != eSceneType.None && CurrentType != NextType)
        {
            DisableScene(CurrentType);

            if(IsAsyc)
            {
                // 비동기 방식
                Operation = SceneManager.LoadSceneAsync(NextType.ToString());
                // 로딩바 생성
                UITools.Inst.ShowLoadingUI(0.0f);

            }
            else
            {
                // 동기 방식
                SceneManager.LoadScene(NextType.ToString());
                CurrentType = NextType;
                NextType = eSceneType.None;
                ComplateLoad(CurrentType);
            }
        }
    }

    void ComplateLoad(eSceneType type)
    {
        UITools.Inst.SubRootCreate();
        switch (type)
        {
            case eSceneType.None:
                break;
            case eSceneType.LogoScene:
                break;
            case eSceneType.BattleScene:
                {
                    GameManger.Inst.LoadBattle();
                    //GameManger.Inst.Initialize();
                    // 씬을 불러올때 Init을 해줘도 괜찮다.
                    //Cam.Inst.FollowCamera();
                }
                break;
            default:
                break;
        }
    }

    // 각 씬에 대한 사용이 종료되면 파괴
    void DisableScene(eSceneType type)
    {
        switch (type)
        {
            case eSceneType.None:
                break;
            case eSceneType.LogoScene:
                break;
            case eSceneType.BattleScene:
                // DisableBattle 실행
                break;
            default:
                break;
        }

        UITools.Inst.Clear();
    }
}
