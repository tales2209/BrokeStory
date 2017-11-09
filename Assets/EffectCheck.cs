using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCheck : MonoBehaviour {

    ParticleSystem Particle;
    float time = 0;
    [SerializeField]
    float destroyTime = 2f;
    void Awake ()
    {
        Particle = GetComponent<ParticleSystem>();
        destroyTime = Particle.main.duration;
        Particle.Play();

	}
	
    void Update ()
    {
        time += Time.deltaTime;
        

        if (time > destroyTime)
            DestroyParticle();

	}

    void DestroyParticle()
    {
        Destroy(gameObject);
    }
}
