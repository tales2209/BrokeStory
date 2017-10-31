using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regenerator : BaseObject
{   
    public eRegenType RegenType = eRegenType.None;
    public int RegenObjectMaximum = 0;
    //public eActorType EnemyType = eActorType.Warrior;

    public float RegenTime = 0;
    public float CurrentTime = 0;
    float Radius;
    

    private void Start()
    {
        switch (RegenType)
        {           
            case eRegenType.Time:
                CurrentTime = 0;
                break;
            case eRegenType.Triger:
               
                break;
        }

        Radius = GetComponent<SphereCollider>().radius;
    }

    private void Update()
    {
        switch (RegenType)
        {
            case eRegenType.Time:
                {
                    CurrentTime += Time.deltaTime;

                    if(CurrentTime >= RegenTime)
                    {
                        CurrentTime = 0;
                        RegenMonster();
                    }
                }
                break;
            case eRegenType.Triger:
                break;
        }
    }

    void RegenMonster()
    {
        int index = 0;

        //GameObject prefabShooter = ActorManager.Inst.GetPrefab(eActorType.Shooter);
        //GameObject PrefabWarrior = ActorManager.Inst.GetPrefab(eActorType.Warrior);

        List<Actor> list = ActorManager.Inst.GetActorList(eTeamType.Blue);

        if (list != null)
            index = list.Count;

        for (int i = index; i < RegenObjectMaximum; ++i)
        {
            Actor actor = null;

            //if (i % 3 == 0)
            //    actor = ActorManager.Inst.InstantiateActor(prefabShooter, SelfTransform.position + RandomPosition());
            //else
            //    actor = ActorManager.Inst.InstantiateActor(PrefabWarrior, SelfTransform.position + RandomPosition());

            ActorManager.Inst.AddActor(actor);
        }
    }

    Vector3 RandomPosition()
    {
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1, 1)).normalized;
        return dir * Random.Range(1, Radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (RegenType)
        {
         
            case eRegenType.Time:
                break;
            case eRegenType.Triger:
                {
                    Actor actor = other.GetComponent<Actor>();

                    if(actor != null && actor.IsPlayer)
                    {
                        RegenMonster();
                    }
                }
                break;
        }
    }    
}