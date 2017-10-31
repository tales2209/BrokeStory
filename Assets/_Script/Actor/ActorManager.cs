using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoSingleton<ActorManager>
{
    Transform ActorTransRoot = null;
    Dictionary<eTeamType, List<Actor>> DicActor = new Dictionary<eTeamType, List<Actor>>();
    Dictionary<eActorType, GameObject> DicPrefabs = new Dictionary<eActorType, GameObject>();

    private void Awake()
    {
        PrefabsLoad();
    }
    
    void PrefabsLoad()
    {
        for(int i =0; i < (int)eActorType.Max; ++i)
        {
            GameObject prefab = Resources.Load("Prefabs/Actor/" + ((eActorType)i).ToString()) as GameObject;
            
            if (prefab == null)
            {
                Debug.Log(((eActorType)i).ToString() + " 로드 실패 오타 확인 하세요.");
            }
            else
                DicPrefabs.Add((eActorType)i, prefab);
        }
    }

    public GameObject GetPrefab(eActorType key)
    {
        if (DicPrefabs.ContainsKey(key) == true)
            return DicPrefabs[key];
        else
        {
            Debug.LogError(key.ToString() + " 프리팹이 없습니다 로드가 되었는지 PrefabsLoad()를 확인하세요.");
            return null;
        }
    }

    public Actor InstantiateActor(GameObject prefab, Vector3 pos)
    {
        if(prefab == null)
        {
            Debug.LogError("프리팹이 없습니다. 제대로 로드가 되었는지 확인하세요.");
            return null;
        }

        if(ActorTransRoot == null)
        {
            GameObject root = new GameObject("ActorRoot");
            ActorTransRoot = root.transform;
        }

        GameObject acotr = Instantiate(prefab, pos, Quaternion.identity);
        acotr.transform.SetParent(ActorTransRoot);

        return acotr.GetComponent<Actor>() ;
    }

    public void AddActor(Actor actor)
    {
        List<Actor> listActor = null;

        if (actor == null)
        {
            Debug.LogError("Actor가 null 입니다");
            return;
        }

        if(DicActor.ContainsKey(actor.TeamType) == false)
        {
            listActor = new List<Actor>();
            DicActor.Add(actor.TeamType, listActor);
        }
        else
        {
            //listActor = DicActor[actor.TeamType];
            DicActor.TryGetValue(actor.TeamType, out listActor);
        }

        listActor.Add(actor);        
    }

    public void RemoveAcotr(Actor actor, bool bDelete = false)
    {
        if(actor == null)
        {
            Debug.LogError("actor가 null 입니다.");
            return;
        }

        if(DicActor.ContainsKey(actor.TeamType) == true)
        {
            List<Actor> listActor = DicActor[actor.TeamType];
            listActor.Remove(actor); // 리스트에서 actor를 지우는것
        }
        else
        {
            Debug.LogError("List 안에 삭제할 Actor가 없습니다. 생각 좀 하고 코딩 하시지 ㅗ");
            return;
        }

        if (bDelete == true)
            Destroy(actor.SelfObject); // actor의 GameObject를 삭제
    }

    public BaseObject GetSerchEnemy(BaseObject actor, out float dist, float radius = 100.0f)
    {
        eTeamType teamType = (eTeamType)actor.GetData(ConstValue.ActorData_GetTeam);

        Vector3 myPos = actor.SelfTransform.position;
        float nearDistance = radius;
        Actor nearActor = null;

        foreach(KeyValuePair<eTeamType, List<Actor>> pair in DicActor)
        {
            if (pair.Key == teamType)
                continue;

            List<Actor> list = pair.Value;

            for(int i=0; i<list.Count; ++i)
            {
                if (list[i].SelfObject.activeSelf == false)
                    continue;

                if (list[i].ObjectState == eBaseState.Die)
                    continue;

                float distance = Vector3.Distance(myPos, list[i].SelfTransform.position);

                if(distance < nearDistance)
                {
                    nearDistance = distance;
                    nearActor = list[i];
                }
            }
        }

        dist = nearDistance;
        return nearActor;
    }

    public Actor GetPlayer()
    {
        List<Actor> list = null;

        DicActor.TryGetValue(eTeamType.Red, out list);

        return list[0];
    }

    public List<Actor> GetActorList(eTeamType teamType)
    {
        List<Actor> list = null;

        DicActor.TryGetValue(teamType, out list);

        return list;
    }
}
