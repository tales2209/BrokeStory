using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    AudioClip audioClip;
    AudioSource SoundObj;
    eSoundType _esoundType = eSoundType.None;

    void Start()
    {
        //soundManager = GetComponent<SoundManager>();

        // Init으로 초기화 작업
    }

    void Update()
    {
        SoundObj.Stop();
    }

    public void OnPlay(AudioClip clip)
    {
        audioClip = clip;
        SoundObj.clip = audioClip;
        SoundObj.Play();
    }
}
