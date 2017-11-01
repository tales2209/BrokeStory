using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BladeInfo
{
    public GameObject Sword;
    public bool Fire;
}

public class BladeWork : Skill
{
    int SwordMax = 12;

    List<BladeInfo> SwordList = new List<BladeInfo>();

    int SwordCnt = 0;

    float Angle = 0;
    float BladeWorkTime = 0;
    float FinishTime = 0;

    bool NextPhase = false;

    GameObject MainPivot;

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

        MainPivot.transform.SetParent(this.transform, false);   // 환영검 최상위 부모는 플레이어의 자식으로 생성된다
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

            yield return new WaitForSeconds(0.1f);
        }
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
            Angle = -180.0f / SwordMax; // (180 / 갯수) = (각도) 마지막 각도 하나가 빠지는 현상
            Angle = (Angle - 180.0f) / SwordMax;    // 제거 하는 식

            index.Sword.transform.parent.rotation = Quaternion.identity;
            index.Sword.transform.parent.rotation = Quaternion.AngleAxis(Angle * count, Vector3.forward);

            index.Sword.transform.localPosition = new Vector3(-1.5f, 0, -1.5f);
            index.Sword.transform.LookAt(Target.position);

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

                sword.Sword.transform.LookAt(Target.position);
            }

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

            yield return new WaitForSeconds(0.15f);
        }

    }

    IEnumerator BladeWorkMove(Transform trans)
    {
        while (true)
        {
            trans.position += trans.forward * 100 * Time.deltaTime;
            yield return null;
        }
    }
}
