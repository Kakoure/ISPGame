using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGolemProjParticles : MonoBehaviour {
    ParticleSystem particleSys;

    void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
    }


    void Update()
    {
        if (!particleSys.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
