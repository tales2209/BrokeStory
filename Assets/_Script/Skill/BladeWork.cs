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
    GameObject Prepap;    

    Transform Target;

    private void Awake()
    {
        Prepap = Resources.Load("Prefabs/Object/Skill/BladeWork") as GameObject;
    }

    void Start ()
    {
        Angle = 360 / SwordMax;        
    }
	
	void Update ()
    {               
        BladeWorkRotation();        
    }

    public void Initialize()
    {
        StartCoroutine(BladeWorkInstantiate());
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
            blade.Sword = Instantiate(Prepap, pivot.transform);
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
            ++count;
        }

        StartCoroutine(BladeWorkFire());
        StartCoroutine(LookAt());
    }

    IEnumerator LookAt()
    {
        float temp;        
        BaseObject enemy = ActorManager.Inst.GetSerchEnemy(ActorManager.Inst.GetPlayer(), out temp);

        if (enemy != null)
            Target = enemy.Getchild("Target");

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
            }

            if (Target == null)
                MainPivot.transform.rotation = Quaternion.LookRotation(transform.forward);
            else
                MainPivot.transform.LookAt(Target.position);

            MainPivot.transform.position = transform.position + new Vector3(0, 0.5f, 0);

            if (SwordCnt <= 0)
            {
                ResetSkill();
                yield break;
            }
            else
                yield return null;
        }
    }

    IEnumerator BladeWorkFire()
    {
        yield return new WaitForSeconds(0.3f);

        int pivot = (SwordMax / 2) - 1;

        for (int i = 0; i <= pivot; ++i)
        {
            StartCoroutine(BladeWorkMove(SwordList[pivot - i].Sword.transform));
            StartCoroutine(BladeWorkMove(SwordList[pivot + i + 1].Sword.transform));

            SwordList[pivot - i].Fire = true;
            SwordList[pivot + i + 1].Fire = true;

            StartCoroutine(DeleteSword(SwordList[pivot - i]));
            StartCoroutine(DeleteSword(SwordList[pivot + i + 1]));

            yield return new WaitForSeconds(0.2f);
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

    IEnumerator DeleteSword(BladeInfo sword)
    {
        Transform trans = sword.Sword.transform;
        Vector3 startPos = trans.position;
        
        while (true)
        {
            if (Vector3.Distance(startPos, trans.position) >= Range)
            {                
                Destroy(trans.gameObject);
                --SwordCnt;
                yield break;
            }

            yield return null;
        }       
    }

    void ResetSkill()
    {
        BladeWorkTime = 0;
        SwordCnt = 0;
        Angle = 360 / SwordMax;
        NextPhase = false;
        SwordList.Clear();
        Destroy(MainPivot);
        ((Player)ActorManager.Inst.GetPlayer()).IsBladeWork = false;
    }
}
