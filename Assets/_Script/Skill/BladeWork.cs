using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BladeInfo
{
    public GameObject Sword;
    public bool Fire;
}

public class BladeWork : MonoBehaviour
{
    int SwordMax = 10;

    List<BladeInfo> SwordList = new List<BladeInfo>();

    int SwordCnt = 0;

    float Angle = 0;
    float BladeWorkTime = 0;
    float FinishTime = 2;
    float Range = 10;

    bool NextPhase = false;

    GameObject MainPivot;
    public Transform Target;   

    void Start ()
    {
        Angle = 360 / SwordMax;
        StartCoroutine(BladeWorkInstantiate());
    }
	
	
	void Update ()
    {               
        BladeWorkRotation();        
    }

    IEnumerator BladeWorkInstantiate()
    {
        BladeInfo blade = null;   // 검
        GameObject pivot = null;    // 검의 상위 부모 이녀석을 회전해야 플레이어 중심으로부터 일정한 각도로 회전 시킨 수 있다
        MainPivot = new GameObject();

        //MainPivot.transform.SetParent(this.transform, false);   // 환영검 최상위 부모는 플레이어의 자식으로 생성된다
        MainPivot.name = "MainPivot";

        for (int i = 0; i < SwordMax; ++i)  // 최대 검 갯수 만큼 생성
        {
            blade = new BladeInfo();
            pivot = new GameObject();
            pivot.name = "Pivot" + i;
            pivot.transform.SetParent(MainPivot.transform, false);       // true false의 차이?
            pivot.transform.rotation = Quaternion.AngleAxis(Angle * i, Vector3.up);

            blade.Sword = Resources.Load("Prefabs/Object/Skill/BladeWork") as GameObject;
            blade.Sword = Instantiate(blade.Sword, pivot.transform);
            blade.Fire = false;
            SwordList.Add(blade);
            ++SwordCnt;
        }

        yield return null;
    }

    void BladeWorkRotation()
    {
        if (NextPhase == true)
            return;

        if (SwordCnt != SwordMax)
            return;

        if (MainPivot == null)
            return;

        MainPivot.transform.Rotate(Vector3.up, 5);

        BladeWorkTime += Time.deltaTime;

        MainPivot.transform.position = transform.position;

        if (BladeWorkTime >= FinishTime)
            BladeWorkFireReady();
    }

    void BladeWorkFireReady()
    {
        int count = 0;
        NextPhase = true;
        MainPivot.transform.rotation = Quaternion.identity;
        

        foreach (BladeInfo index in SwordList)
        {
            Angle = -180 / (SwordMax-1); // (180 / 갯수) = (각도) 마지막 각도 하나가 빠지는 현상            

            index.Sword.transform.parent.rotation = Quaternion.identity;
            index.Sword.transform.parent.rotation = Quaternion.AngleAxis(Angle * count, Vector3.forward);

            index.Sword.transform.localPosition = new Vector3(-1.0f, 0.0f, -1.0f);
            //index.Sword.transform.LookAt(Target.position);

            ++count;
        }

        StartCoroutine(BladeWorkFire());
        StartCoroutine(LookAt());
    }

    IEnumerator LookAt()
    {
        while (true)
        {
            foreach (BladeInfo sword in SwordList)
            {
                if (sword.Fire)
                    continue;

                if (Target == null)
                    sword.Sword.transform.rotation = Quaternion.LookRotation(transform.forward);
                else
                    sword.Sword.transform.LookAt(Target.position);

                Debug.Log(this.transform.forward);
            }

            if (Target == null)
                MainPivot.transform.rotation = Quaternion.LookRotation(transform.forward);
            else
                MainPivot.transform.LookAt(Target.position);

            MainPivot.transform.position = transform.position + new Vector3(0, 0.5f, 0);            

            if (SwordList.Count == 0)
                yield break;
            else
                yield return null;
        }
    }
    IEnumerator BladeWorkFire()
    {
        int pivot = (SwordMax / 2) - 1;

        for (int i = 0; i <= pivot; ++i)
        {
            StartCoroutine(BladeWorkMove(SwordList[pivot - i].Sword.transform));
            StartCoroutine(BladeWorkMove(SwordList[pivot + i + 1].Sword.transform));

            SwordList[pivot - i].Fire = true;
            SwordList[pivot + i + 1].Fire = true;

            StartCoroutine(DeleteSword(SwordList[pivot - i].Sword.transform));
            StartCoroutine(DeleteSword(SwordList[pivot + i + 1].Sword.transform));

            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator BladeWorkMove(Transform trans)
    {
        while (true)
        {
            if (trans == null)
                yield break;

            if (Target == null)
                trans.position += this.transform.forward * 100 * Time.deltaTime;
            else
                trans.position += trans.forward * 100 * Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator DeleteSword(Transform sword)
    {
        Vector3 startPos = sword.position;
        
        while (true)
        {
            if (Vector3.Distance(startPos, sword.position) >= Range)
            {
                Destroy(sword.gameObject);
                yield break;
            }

            yield return null;
        }       
    }
}
