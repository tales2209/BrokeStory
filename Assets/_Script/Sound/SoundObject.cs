using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    AudioClip audioClip;
    AudioSource SoundObj;
    eSoundType _esoundType = eSoundType.None;

    // Start로 해당 변수에 컴포넌트를 적용시키기 전에 
    // OnPlay를 먼저 실행 시킴으로써 실행에 오류가 생김
    void Start()
    {
        // Init으로 초기화 작업
    }

    void Update()
    {
        //SoundObj.Stop();
    }

    public void OnPlay(AudioClip clip)
    {
        audioClip = GetComponent<AudioClip>();
        SoundObj = GetComponent<AudioSource>();

        if (_esoundType == eSoundType.BackGround)
            SoundObj.loop = true;

        audioClip = clip;
        SoundObj.clip = audioClip;
        SoundObj.Play();
    }
}
