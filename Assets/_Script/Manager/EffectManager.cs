using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoSingleton<EffectManager>
{
    Dictionary<string, GameObject> EffectPrefab = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> EffectDic = new Dictionary<string, GameObject>();

    public void LoadEffect(string path, string name)
    {
        GameObject effect = Resources.Load(path + name) as GameObject;

        if(EffectPrefab.ContainsKey(name))
        {
            Debug.Log(name + "의 이펙트 프리팹이 두번 로드 되고 있습니다. 확인 하세요 ㅗ");
            return;
        }
            
        EffectPrefab.Add(name, effect);
    }

    public GameObject InitEffect(string name, Transform trans)
    {
        GameObject effect;

        if (!EffectPrefab.TryGetValue(name, out effect))
        {
            Debug.Log(name + "프리팹이 없습니다");
            return null;
        }

        effect = Instantiate(effect, trans.position, Quaternion.identity);
        effect.transform.SetParent(trans);
        EffectDic.Add(name, effect);

        return effect;
    }

    public void AutoPlayEffect(string name, Transform trans, bool IsHas = true)
    {
        GameObject effect;

        if (!EffectPrefab.TryGetValue(name, out effect))
        {
            Debug.Log(name + "프리팹이 없습니다");
            return;
        }

        effect = Instantiate(effect, trans.position, Quaternion.identity);

        if (IsHas)
            effect.transform.SetParent(trans);
        else
            StartCoroutine(AutoDelete(effect));
    }

    public GameObject GetEffect(string key)
    {
        GameObject effect;

        if (!EffectDic.TryGetValue(key, out effect))
        {
            Debug.Log(name + "프리팹이 없습니다");            
        }

        return effect;
    }

    IEnumerator AutoDelete(GameObject effect, float loopCnt = 1)
    {
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        
        while(true)
        {
            if (ps.time >= loopCnt)
            {
                Destroy(effect);
                yield break;
            }

            yield return null;
        }
    }
    
    public void DestroyEffect(string key)
    {
        GameObject effect = GetEffect(key);
        EffectDic.Remove(key);
        Destroy(effect);
    }

    public void RemoveEffect(string key)
    {
        EffectDic.Remove(key);
    }
}
