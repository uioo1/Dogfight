using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public ParticleSystem particle;
    public bool isHalfShotted;
    public bool isFullShotted;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        isHalfShotted = false;
        isFullShotted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHalfShotted)
        {
            particle.Play(true);
            //particle.emission.enabled = true;
        }
        else if(isFullShotted)
        {
            //particle.Play(true);
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        else
        {
            //particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
