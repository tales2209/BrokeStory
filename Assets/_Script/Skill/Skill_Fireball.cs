using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Fireball : MonoBehaviour {

    float time = 0;
    float aniTime = 0;
    public float Height = 0;
    public float updownSpeed = 1f;
    public float shakePower = 0.5f;
    public float movingSpeed = 1f;
    Vector3 orgPos;
    GameObject Particle = null;
    
    List<Skill_Fireball> list_Fireball; //생성될때 추가된 리스트를 받아오기위한 변수



    private void Awake()
    {
        Particle = Resources.Load("Prefabs/Object/Effect/" + "FireballParticle") as GameObject;
        orgPos = transform.position;    //최초 생성위치 저장
    }


    //해당 파이어볼을 생성할때 담아놓은 리스트를 파이어볼 자신도 알수있게 하기위한 메소드
    public void SetListFireBall(List<Skill_Fireball> list)
    {
        list_Fireball = list;
        
    }

    void Update ()
    {
        time += Time.deltaTime;
        aniTime += Time.deltaTime;

        Vector3 M_height = orgPos + new Vector3(0, Height, 0);
        

        //생성한뒤 (height 크기만큼)위로 떠오르게 하는 처리
        if(time <= 1)
        {
        transform.position = Vector3.Lerp(transform.position, M_height,  time * updownSpeed);
            if(time >= 0.99f)
            orgPos = transform.position;
        }
        if(aniTime >= 1f)
        {
            aniTime = 0;
        }

        if (time > 1 && time <= 5 && aniTime >= 0.5f )
        {
            
            Vector3 RandomPos = transform.position + new Vector3(Random.Range(-shakePower, shakePower), 0, Random.Range(-shakePower, shakePower));
            if (Vector3.Distance(orgPos, RandomPos) < shakePower * 10f && Vector3.Distance(orgPos, RandomPos) > transform.localScale.z  )
            {
                transform.position = RandomPos;
            }
        }



        if (time > 5)
        {
            transform.position += transform.forward * movingSpeed;


        }
        if(time > 10)
        {
            DestroyFireball();
        }
	}


    void DestroyFireball()
    {
        //파이어볼의 게임오브젝트를 삭제하기전 먼저 본인이 담겨있는 리스트에서 삭제를 진행하고 게임오브젝트 파괴한다
        for (int i = 0; i < list_Fireball.Count; i++)
        {
            if (list_Fireball[i].gameObject.Equals(gameObject))
                list_Fireball.Remove(list_Fireball[i]);
        }
        Destroy(gameObject);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        
        //충돌한 게임오브젝트가 플레이어이고 파이어볼 생성된지 5초가 지났으면 데미지처리 & 파이어볼 삭제
        if( other.gameObject == GameObject.FindGameObjectWithTag("Player") && time > 5)
        {
          Actor otherActor =  other.GetComponent<Actor>();
            otherActor.ThrowEvent(ConstValue.EventKey_Hit);
            Instantiate(Particle, transform.position, Quaternion.identity);

            DestroyFireball();

        }
        //other.GetComponent<Rigidbody>().AddForce(Vector3.forward * 100);
        //if(other.GetComponent<Actor>().IsPlayer ==true)
        //{
        //    Destroy(other.gameObject);
        //}
    }


}
