using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;

    // 클립은 재생해야할 오디오 파일들의 집합?
    // 소스는 실제로 재생할 객체
    Dictionary<eSoundType, AudioClip> audioDic = new Dictionary<eSoundType, AudioClip>();
    List<SoundObject> SourceList = new List<SoundObject>();

    GameObject audioSourcePrefab = null;
    private void Awake()
    {
        _instance = this;
        audioSourcePrefab = Resources.Load("Prefabs/Sound/" + "SoundObject") as GameObject;
    }

    // 이넘형을 매개변수로 사용해 해당 사운드타입에 로드와 플레이 작업을 해준다.
    public void AudioPlay(eSoundType esoundType)
    {
        AudioClip c = ClipLoad(esoundType);
        SoundObject Source = GetAudioSource();
        Source.OnPlay(c);
    }

    // 사운드 타입을 매개변수로 해당 타입이 있으면 반환
    // 없으면 로드해와 c에 넣어주고 딕셔너리에 저장한다
    AudioClip ClipLoad(eSoundType esoundType)
    {
        if (audioDic.ContainsKey(esoundType) == true)
        {
            return audioDic[esoundType];
        }

        AudioClip c = Resources.Load("Prefabs/Sound/_Audio/" + esoundType.ToString()) as AudioClip;

        if (c == null)
        {
            Debug.Log(" 불러올 오디오클립이 없다. ");
            return null;
        }

        // 딕셔너리에 esoundType으로 c가 저장
        audioDic.Add(esoundType, c);
        return c;
    }

    SoundObject GetAudioSource()
    {
        SoundObject source = null;
        for (int i = 0; i < SourceList.Count; i++)
        {
            source = SourceList[i];
            if (source.gameObject.activeSelf == false)
            {
                source.gameObject.SetActive(true);
                return source;
            }
        }

        // 없다
        GameObject go = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity);
        source = go.GetComponent<SoundObject>();

        // 리스트에 source가 저장
        SourceList.Add(source);
        return source;
    }


}
